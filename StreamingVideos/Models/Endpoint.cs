using System;
using System.Collections.Generic;
using System.Text;

namespace StreamingVideos.Models
{
    class Endpoint
    {
        public int Id { get; set; }
        public int LatencyToDataCenter { get; set; }
        public int CacheCount { get; set; }
        public Dictionary<int, int> CacheServers { get; set; }

        public Dictionary<int, int> Requests { get; set; }
    }
}
