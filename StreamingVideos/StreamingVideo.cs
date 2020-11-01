using System;
using System.Collections.Generic;
using System.Text;
using StreamingVideos.Models;
using System.Linq;  

namespace StreamingVideos
{
    class StreamingVideo
    {
        public int NumberOfVideos { get; set; }
        public int NumberOfCaches { get; set; }
        public int NumberOfEndpoints { get; set; }
        public int NumberOfRequests { get; set; }
        public int CacheSize { get; set; }

        public List<Video> Videos { get; set; }

        public List<CacheServer> CacheServers { get; set; }

        public List<Endpoint> Endpoints { get; set; }


        public StreamingVideo()
        {
            Videos = new List<Video>();
            CacheServers = new List<CacheServer>();
            Endpoints = new List<Endpoint>();
        }

        public void Init(string[] data)
        {
            NumberOfVideos = Convert.ToInt32(data[0]);
            NumberOfEndpoints = Convert.ToInt32(data[1]);
            NumberOfRequests = Convert.ToInt32(data[2]);
            NumberOfCaches = Convert.ToInt32(data[3]);
            CacheSize = Convert.ToInt32(data[4]);
        }

        public void InitCacheServers()
        {
            for (int i = 0; i < NumberOfCaches; i++)
            {
                var cache = new CacheServer()
                {
                    Id = i,
                    Capacity = CacheSize
                };

                CacheServers.Add(cache);
            }
        }

        public void InitVideos(string[] input)
        {
            for (int i = 0; i < NumberOfVideos; i++)
            {
                var video = new Video()
                {
                    Id = i,
                    Size = Convert.ToInt32(input[i])
                };

                Videos.Add(video);
            }
        }

        public void Compute()
        {
            Console.WriteLine("Computing");
        }
    }
}
