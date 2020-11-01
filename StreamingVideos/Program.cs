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

            Parser dataParser = new Parser(basePath + dataset);
            StreamingVideo sv = new StreamingVideo();

            dataParser.ParseData(sv);

            sv.Compute();
        }
    }
}
