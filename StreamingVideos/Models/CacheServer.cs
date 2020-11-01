using System;
using System.Collections.Generic;
using System.Text;

namespace StreamingVideos.Models
{
    class CacheServer
    {
        public int Id { get; set; }
        public int Capacity { get; set; }

        public List<Video> Videos { get; set; }
    }
}
