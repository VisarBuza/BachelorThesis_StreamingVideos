using System.Collections.Generic;

namespace StreamingVideos.Models
{
    public class DataModel
    {
        // Range(1, 10000)
        public int NumVideos { get; set; }
        // Range(1, 1000)
        public int NumEndpoints { get; set; }
        // Range(1, 1000000)
        public int NumRequests { get; set; }
        // Range(1, 1000)
        public int NumCaches { get; set; }
        // Range(1, 500000)
        public int CacheCapacity { get; set; }

        public List<int> VideoSizes { get; set; } = new();

        public List<Endpoint> Endpoints { get; set; } = new();

        public List<Request> Requests { get; set; } = new();

    }
}
