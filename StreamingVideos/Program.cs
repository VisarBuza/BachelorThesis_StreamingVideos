using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamingVideos
{
    static class Program
    {
        static void Main(string[] args)
        {
            var sv = new StreamingVideoService();
            var dataParser = new Parser(@"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\example.in", sv);
            
            dataParser.ParseData();
            sv.Compute();
            LogData(sv);
        }

        static void LogData(StreamingVideoService sv)
        {
            sv.Videos.ForEach(x => Console.WriteLine($"Video with id {x.Id} size : {x.Size}"));
            
            foreach (var endpoint in sv.Endpoints)
            {
                Console.WriteLine("Endpoint: " + endpoint.Id + " caches:"
                                  + endpoint.CacheCount + " latency" + endpoint.LatencyToDataCenter);

                foreach (var item in endpoint.CacheServers)
                {
                    Console.WriteLine("Cache id :" + item.Key + " cache latency :" + item.Value);
                }

                foreach (var item in endpoint.Requests)
                {
                    Console.WriteLine("Video id :" + item.Key + " Number Of Requests: " + item.Value);
                }
            }
        }
    }
}
