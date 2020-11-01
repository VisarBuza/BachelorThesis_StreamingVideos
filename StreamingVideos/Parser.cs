using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StreamingVideos.Models;

namespace StreamingVideos
{
    class Parser
    {
        private string _file;

        public Parser(string file) => _file = file;


        public void ParseData(StreamingVideo sv)
        {
            using (StreamReader sr = new StreamReader(_file))
            {
                Console.WriteLine(sr.ReadLine());
            }
        }

    }
}
