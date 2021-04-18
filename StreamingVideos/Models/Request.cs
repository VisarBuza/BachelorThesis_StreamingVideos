namespace StreamingVideos.Models
{
    public class Request
    {
        public int Video { get; set; }
        public int Endpoint { get; set; }
        public int RequestNo { get; set; }
        public int LowestLatency { get; set; } = int.MaxValue;
    }
}