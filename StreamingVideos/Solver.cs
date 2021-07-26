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
        private readonly Random random = new();

        public void Init(DataModel dataModel)
        {
            _dataModel = dataModel;
        }

        public void Solve()
        {  
            var cacheServers = FillCacheServers();
            
            var currentScore = CalculateScore(cacheServers);

            Console.WriteLine($"Initial Solution : {currentScore}");
            
            var sw = new Stopwatch();

            sw.Start();

            while (sw.ElapsedMilliseconds < 30000)
            {
                var server = GetRandomCache();
                
                if (cacheServers[server].Count < 1) continue;
                
                var videoToAdd = GetRandomVideo(cacheServers[server]);
                
                var videoToRemove = random.Next(0, cacheServers[server].Count);

                var latencyGains = LatencyGains(videoToAdd, server);

                var latencyGainsOfRemovedVideo = LatencyGains(cacheServers[server][videoToRemove], server);
                
                if (latencyGains > latencyGainsOfRemovedVideo)
                {
                    cacheServers[server][videoToRemove] = videoToAdd;
                }
            }

            Console.WriteLine($"Final score: {CalculateScore(cacheServers)}");
        }

        private List<int>[] FillCacheServers()
        {
            var cacheServers = Enumerable.Range(0, _dataModel.NumCaches).Select(_ => new List<int>()).ToArray();

            foreach (var cache in cacheServers)
            {
                var size = 0;
                while (true)
                {
                    var index = random.Next(0, _dataModel.NumVideos);
                   
                    var videoSize = _dataModel.VideoSizes[index];
                    
                    size += videoSize;

                    if (size > _dataModel.CacheCapacity)
                        break;

                    if (cache.Contains(index)) continue;

                    cache.Add(index);
                }
            }

            return cacheServers;
        }

        private long CalculateScore(List<int>[] cacheServers)
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

        public void LogData()
        {
            _dataModel.VideoSizes.ForEach(x => Console.WriteLine($"Video with id {x} size : {x}"));

            foreach (var endpoint in _dataModel.Endpoints)
            {
                Console.WriteLine("Endpoint: " + endpoint.Id + " caches:"
                                  + endpoint.CacheCount + " latency" + endpoint.LatencyToDataCenter);

                foreach (var item in endpoint.CacheServers)
                {
                    Console.WriteLine("Cache id :" + item.Key + " cache latency :" + item.Value);
                }
            }

            _dataModel.Requests.ForEach(x => Console.WriteLine($"Video {x.Video} is requested by endpoint {x.Endpoint}, {x.RequestNo} times"));
        }

        public int GetRandomVideo(List<int> cache)
        {
            var videosNotInCache = Enumerable.Range(0, _dataModel.NumVideos).Except(cache).ToList();
            return videosNotInCache[random.Next(0, videosNotInCache.Count)];
        }

        public int GetRandomCache()
        {
            return random.Next(0, _dataModel.NumCaches);
        }
    }
}