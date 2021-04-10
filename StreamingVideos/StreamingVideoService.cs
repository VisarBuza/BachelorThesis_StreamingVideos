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
        

        public void Init(string[] data)
        {
            NumberOfVideos = int.Parse(data[0]);
            NumberOfEndpoints = int.Parse(data[1]);
            NumberOfRequests = int.Parse(data[2]);
            NumberOfCaches = int.Parse(data[3]);
            CacheSize = int.Parse(data[4]);
        }

        public void InitVideos(IEnumerable<string> input)
        {
            input.Select((value, index) => new { value, index}).ToList()
                .ForEach(x => Videos.Add(new Video { Id = x.index, Size = int.Parse(x.value) }));
        }

        public void Compute()
        {
            var cacheServers = new List<Video>[NumberOfCaches];

            Console.WriteLine("Computing");
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
