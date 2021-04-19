using System;
using System.Collections.Generic;
using System.Linq;
using StreamingVideos.Models;

namespace StreamingVideos
{
    public class StreamingVideoService
    {
        public int NumberOfVideos { get; set; }
        public int NumberOfEndpoints { get; set; }
        public int NumberOfRequests { get; set; }
        public int NumberOfCaches { get; set; }
        public int CacheSize { get; set; }

        public List<Video> Videos { get; set; } = new ();
        public List<Request> Requests { get; set; } = new();
        public List<Endpoint> Endpoints { get; set; } = new();

        public int AllRequests { get; set; }


        private readonly Random random = new();
        
        public void Init(string[] data)
        {
            NumberOfVideos = int.Parse(data[0]);
            NumberOfEndpoints = int.Parse(data[1]);
            NumberOfRequests = int.Parse(data[2]);
            NumberOfCaches = int.Parse(data[3]);
            CacheSize = int.Parse(data[4]);
        }

        public void SetVideoSizes(IEnumerable<string> input)
        {
            input.Select((value, index) => new { value, index}).ToList()
                .ForEach(x => Videos.Add(new Video { Id = x.index, Size = int.Parse(x.value) }));
        }

        public void Compute()
        {
            var cacheServers = InitialValuation();
            var currentScore = CalculateScore(cacheServers);
            var i = 1000000;
            
            while (true)
            {
                var temp = InitialValuation();
                var tempScore = CalculateScore(temp);

                if (tempScore > currentScore)
                {
                    currentScore = tempScore;
                    cacheServers = temp;
                }
                    
            
                if (i-- < 0) break;
            }

            Console.WriteLine($"Computing {currentScore}");
        }

        private Dictionary<int, Video>[] InitialValuation()
        {
            var cacheServers = Enumerable.Range(0, NumberOfCaches).Select(_ => new Dictionary<int, Video>()).ToArray();

            foreach (var cache in cacheServers)
            {
                var index = random.Next(0, Videos.Count);
                var size = 0;
                while (true)
                {
                    var video = Videos[index % Videos.Count];
                    size += video.Size;
                    index++;
                    
                    if (size > CacheSize) break;
                    cache[video.Id] = video;
                }
            }
            
            return cacheServers;
        }
        
        private int CalculateScore(Dictionary<int, Video>[] cacheServers)
        {
            var score = 0;
            
           
            foreach (var request in Requests)
            {
                var cacheIds = Endpoints[request.Endpoint].CacheServers.Keys.ToArray();
                foreach (var index in cacheIds)
                {
                    if (!cacheServers[index].ContainsKey(request.Video)) continue;
                    
                    if (Endpoints[request.Endpoint].CacheServers[index] < request.LowestLatency)
                    {
                        request.LowestLatency = Endpoints[request.Endpoint].CacheServers[index];
                    }
                }

                score += request.RequestNo * (Endpoints[request.Endpoint].LatencyToDataCenter - request.LowestLatency);
            }
            
            return score * 1000 / AllRequests;
        }
        
        public void LogData()
        {
            Videos.ForEach(x => Console.WriteLine($"Video with id {x.Id} size : {x.Size}"));
            
            foreach (var endpoint in Endpoints)
            {
                Console.WriteLine("Endpoint: " + endpoint.Id + " caches:"
                                  + endpoint.CacheCount + " latency" + endpoint.LatencyToDataCenter);

                foreach (var item in endpoint.CacheServers)
                {
                    Console.WriteLine("Cache id :" + item.Key + " cache latency :" + item.Value);
                }
            }

            Requests.ForEach(x => Console.WriteLine($"Video {x.Video} is requested by endpoint {x.Endpoint}, {x.RequestNo} times"));
        }
    }
}
