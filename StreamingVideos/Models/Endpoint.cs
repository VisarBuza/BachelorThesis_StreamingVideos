using System.Collections.Generic;

namespace StreamingVideos.Models
{
    public class Endpoint
    {
        public int Id { get; set; }
        
        public int LatencyToDataCenter { get; set; }
        
        public int CacheCount { get; set; }

        public Dictionary<int, int> CacheServers { get; set; } = new();
    }
}
