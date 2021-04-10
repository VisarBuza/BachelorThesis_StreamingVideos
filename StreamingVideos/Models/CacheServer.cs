using System.Collections.Generic;

namespace StreamingVideos.Models
{
    public class CacheServer
    {
        public int Id { get; set; }
        
        public int Capacity { get; set; }
        
        public List<Video> Videos { get; set; }
    }
}
