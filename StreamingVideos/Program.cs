using Microsoft.Extensions.Configuration;
using System;

namespace StreamingVideos
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = "C:\\Programming\\BachelorThesis_StreamingVideos\\StreamingVideos\\dataset\\";

            Parser dataParser = new Parser(basePath + args[0]);

            StreamingVideo sv = new StreamingVideo();

            dataParser.ParseData(sv);

            sv.Compute();
        }
    }
}
