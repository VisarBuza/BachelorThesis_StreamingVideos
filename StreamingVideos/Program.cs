using Microsoft.Extensions.Configuration;
using System;

namespace StreamingVideos
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = Environment.GetEnvironmentVariable("BasePath");
            string dataset = args[0].Trim();

            StreamingVideo sv = new StreamingVideo();
            Parser dataParser = new Parser(basePath + dataset, sv);

            dataParser.ParseData();
            sv.Compute();
        }

        static void LogData(StreamingVideo sv)
        {
            foreach (var video in sv.Videos)
            {
                Console.WriteLine("Video: " + video.Id + " Size:" + video.Size);
            }

            foreach (var cache in sv.CacheServers)
            {
                Console.WriteLine("Cache: " + cache.Id + " Capacity " + cache.Capacity);
            }

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
