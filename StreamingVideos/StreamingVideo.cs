using System;
using System.Collections.Generic;
using System.Text;
using StreamingVideos.Models;

namespace StreamingVideos
{
    class StreamingVideo
    {
        public int nVideos;
        public int nCaches;
        public int nEndpoints;
        public int nRequests;

        public List<Video> Videos { get; set; }

        public List<CacheServer> CacheServers { get; set; }

        public List<Endpoint> Endpoints { get; set; }


        public StreamingVideo() {}

        public void Compute()
        {
            Console.WriteLine("Calculating");
        }
    }
}
