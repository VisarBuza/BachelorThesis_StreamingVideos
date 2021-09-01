using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StreamingVideos.Models;

namespace StreamingVideos
{
    public class Solver
    {
        private DataModel _dataModel;
        private readonly Dictionary<string, int> _gainCache = new();
        private readonly List<int> _cacheSizes = new();
        private readonly Random random = new();
        public string dataset;

        public Solver(string dataset)
        {
            this.dataset = dataset;
        }

        public void Init(DataModel dataModel)
        {
            _dataModel = dataModel;
        }

        public void Solve()
        {  
            var cacheServers = FillCacheServers();
            
            var currentScore = CalculateScore(cacheServers);
            var currentSolution = cacheServers;

            Console.WriteLine($"{dataset} Initial Solution : {currentScore}");
            
            var sw = new Stopwatch();

            sw.Start();

            var bestScore = currentScore;

            while (sw.Elapsed < TimeSpan.FromMinutes(3))
            {
                var server = GetRandomCache();

                if (cacheServers[server].Count < 1)
                {
                    var video = GetRandomVideo(cacheServers[server], _dataModel.CacheCapacity);

                    cacheServers[server].Add(video);

                    _cacheSizes[server] += _dataModel.VideoSizes[video];

                    continue;
                }

                var number = random.Next(0, 100);
                
                if (number > 90)
                {
                    SwapRandom(cacheServers);
                }
                else if (number > 50)
                {
                    SwapOne(cacheServers);
                }
                else
                {
                    SwapTwo(cacheServers);
                }

                if (sw.Elapsed < TimeSpan.FromSeconds(60) && sw.Elapsed > TimeSpan.FromSeconds(30))
                {
                    currentScore = CalculateScore(cacheServers);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        currentSolution = cacheServers;
                    }

                    cacheServers = FillCacheServers();
                }
                else if (sw.Elapsed < TimeSpan.FromMinutes(3) && sw.Elapsed > TimeSpan.FromMinutes(2))
                {
                    currentScore = CalculateScore(cacheServers);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        currentSolution = cacheServers;
                    }

                    cacheServers = FillCacheServers();
                }
            }

            currentScore = CalculateScore(cacheServers);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                currentSolution = cacheServers;
            }

            Console.WriteLine($"{dataset} Final score: {bestScore}");

            foreach (var cache in currentSolution)
            {
                var sum = cache.Sum(video => _dataModel.VideoSizes[video]);

                if (sum > _dataModel.CacheCapacity)
                {
                    Console.WriteLine($"{dataset} Size " + sum + " Capacity: " + _dataModel.CacheCapacity + " Count: " + cache.Count);
                }
            }
        }

        public void SwapRandom(HashSet<int>[] cacheServers)
        {
            var server = GetRandomCache();

            var videoToRemove = cacheServers[server].ToList()[random.Next(0, cacheServers[server].Count)];

            var videoToAdd = GetRandomVideo(cacheServers[server],
                _dataModel.CacheCapacity - _cacheSizes[server] - _dataModel.VideoSizes[videoToRemove]);

            if (videoToAdd == -1) return;

            var latencyGains = LatencyGains(videoToAdd, server);

            var latencyGainsOfRemovedVideo = LatencyGains(videoToRemove, server);

            if (latencyGains > latencyGainsOfRemovedVideo)
            {
                cacheServers[server].Remove(videoToRemove);
                cacheServers[server].Add(videoToAdd);

                _cacheSizes[server] += _dataModel.VideoSizes[videoToAdd] - _dataModel.VideoSizes[videoToRemove];
            }
        }

        public void SwapOne(HashSet<int>[] cacheServers)
        {
            var cache1 = GetRandomCache();
            var cache2 = GetRandomCache();

            if (cache1 == cache2) return;

            var firstCacheContents = cacheServers[cache1].Except(cacheServers[cache2]).OrderByDescending(video => LatencyGains(video, cache1)).ToList();
            var secondCacheContents = cacheServers[cache2].Except(cacheServers[cache1]).OrderByDescending(video => LatencyGains(video, cache2)).ToList();

            if (firstCacheContents.Count < 1 || secondCacheContents.Count < 1) return;

            var worstVideoInCache1 = firstCacheContents.Last();
            var worstVideoInCache2 = secondCacheContents.Last();

            var cache1AdditionalSize = _dataModel.VideoSizes[worstVideoInCache2] - _dataModel.VideoSizes[worstVideoInCache1];
            var cache2AdditionalSize = _dataModel.VideoSizes[worstVideoInCache1] - _dataModel.VideoSizes[worstVideoInCache2];


            if (_cacheSizes[cache1] + cache1AdditionalSize > _dataModel.CacheCapacity) return;
            
            if (_cacheSizes[cache2] + cache2AdditionalSize > _dataModel.CacheCapacity) return;

            var gains = LatencyGains(worstVideoInCache1, cache2) + LatencyGains(worstVideoInCache2, cache1);

            var gainsOfRemoved = LatencyGains(worstVideoInCache1, cache1) + LatencyGains(worstVideoInCache2, cache2);

            if (gains > gainsOfRemoved)
            {
                cacheServers[cache1].Remove(worstVideoInCache1);
                cacheServers[cache1].Add(worstVideoInCache2);

                cacheServers[cache2].Remove(worstVideoInCache2);
                cacheServers[cache2].Add(worstVideoInCache1);


                _cacheSizes[cache1] += cache1AdditionalSize;
                _cacheSizes[cache2] += cache2AdditionalSize;
            }
        }

        public void SwapTwo(HashSet<int>[] cacheServers)
        {
            var cache1 = GetRandomCache();
            var cache2 = GetRandomCache();

            if (cache1 == cache2) return;

            var videos1 = cacheServers[cache1].Except(cacheServers[cache2]).OrderByDescending(x => LatencyGains(x, cache1)).ToList();
            var videos2 = cacheServers[cache2].Except(cacheServers[cache1]).OrderByDescending(x => LatencyGains(x, cache2)).ToList();

            if (videos1.Count < 2 || videos2.Count < 2) return;

            var video1cache1 = videos1[^2];
            var video2cache1 = videos1[^1];

            var video1cache2 = videos2[^2];
            var video2cache2 = videos2[^1];

            var cache1AdditionalSize = _dataModel.VideoSizes[video1cache2] + _dataModel.VideoSizes[video2cache2] -
                                       _dataModel.VideoSizes[video1cache1] - _dataModel.VideoSizes[video2cache1];

            var cache2AdditionalSize = _dataModel.VideoSizes[video1cache1] + _dataModel.VideoSizes[video2cache1] -
                                       _dataModel.VideoSizes[video1cache2] - _dataModel.VideoSizes[video2cache2];

            if (_cacheSizes[cache1] + cache1AdditionalSize > _dataModel.CacheCapacity) return;
            
            if (_cacheSizes[cache2] + cache2AdditionalSize > _dataModel.CacheCapacity) return;

            var gains = LatencyGains(video1cache1, cache2) + LatencyGains(video2cache1, cache2) +
                   LatencyGains(video1cache2, cache1) +LatencyGains(video2cache2, cache1);

            var gainsOfRemoved = LatencyGains(video1cache1, cache1) + LatencyGains(video2cache1, cache1) +
                            LatencyGains(video1cache2, cache2) + LatencyGains(video2cache2, cache2);

            if (gains > gainsOfRemoved)
            {
                cacheServers[cache1].Remove(video1cache1);
                cacheServers[cache1].Remove(video2cache1);
                cacheServers[cache2].Remove(video1cache2);
                cacheServers[cache2].Remove(video2cache2);

                cacheServers[cache1].Add(video1cache2);
                cacheServers[cache1].Add(video2cache2);
                cacheServers[cache2].Add(video1cache1);
                cacheServers[cache2].Add(video2cache1);

                _cacheSizes[cache1] += cache1AdditionalSize;
                _cacheSizes[cache2] += cache2AdditionalSize;
            }
        }

        private HashSet<int>[] FillCacheServers()
        {
            var cacheServers = Enumerable.Range(0, _dataModel.NumCaches).Select(_ => new HashSet<int>()).ToArray();

            _cacheSizes.Clear();

            foreach (var cache in cacheServers)
            {
                var size = 0;

                while (true)
                {
                    var index = random.Next(0, _dataModel.NumVideos);
                   
                    var videoSize = _dataModel.VideoSizes[index];

                    if (size + videoSize > _dataModel.CacheCapacity)
                        break;

                    if (cache.Contains(index))
                        continue;

                    cache.Add(index);

                    size += videoSize;
                }

                _cacheSizes.Add(size);
            }

            return cacheServers;
        }

        private long CalculateScore(HashSet<int>[] cacheServers)
        {
            long score = 0;

            foreach (var request in _dataModel.Requests)
            {
                var endpoint = request.Endpoint;
                var video = request.Video;
                var numOfRequest = request.RequestNo;

                foreach (var cacheId in _dataModel.Endpoints[endpoint].CacheServers.Keys.ToArray())
                {
                    if (!cacheServers[cacheId].Contains(video)) continue;

                    if (_dataModel.Endpoints[request.Endpoint].CacheServers[cacheId] < request.LowestLatency)
                    {
                        request.LowestLatency = _dataModel.Endpoints[request.Endpoint].CacheServers[cacheId];
                    }
                }

                score += numOfRequest * (_dataModel.Endpoints[request.Endpoint].LatencyToDataCenter - request.LowestLatency);
            }

            long totalRequests = _dataModel.Requests.Sum(x => x.RequestNo);

            return score / totalRequests * 1000;
        }

        public int LatencyGains(int videoId, int cacheId)
        {
            var cacheKey = $"{videoId}:{cacheId}";
            
            if (_gainCache.ContainsKey(cacheKey)) return _gainCache[cacheKey];

            var score = 0;

            foreach (var request in _dataModel.Requests.Where(x => x.Video == videoId))
            {
                var endpoint = request.Endpoint;
                
                if (!_dataModel.Endpoints[endpoint].CacheServers.ContainsKey(cacheId)) continue;

                var numOfRequest = request.RequestNo;

                var latencyToCacheServer = _dataModel.Endpoints[endpoint].CacheServers[cacheId];
                
                score += numOfRequest * (_dataModel.Endpoints[request.Endpoint].LatencyToDataCenter - latencyToCacheServer);
            }

            _gainCache[cacheKey] = score;

            return score;
        }

        public int GetRandomVideo(HashSet<int> cache, int maximumAllowedSize)
        {
            var videosNotInCache = Enumerable.Range(0, _dataModel.NumVideos).Except(cache).ToList();

            var availableVideos = _dataModel.VideoSizes.Select((size, id) => new { size, id })
                .Where(video => video.size <= maximumAllowedSize)
                .Select(video => video.id);

            var videos = videosNotInCache.Intersect(availableVideos).ToList();

            return videos.Count > 0 ? videos[random.Next(0, videos.Count)] : -1;
        }

        public int GetRandomCache()
        {
            return random.Next(0, _dataModel.NumCaches);
        }
    }
}