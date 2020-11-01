using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StreamingVideos.Models;
using System.Linq;

namespace StreamingVideos
    {
    class Parser
    {
        private readonly string _file;
        private readonly StreamingVideo _sv;

        public Parser(string file, StreamingVideo sv)
        {
            _file = file;
            _sv = sv;
        }


        public void ParseData()
        {
            Console.WriteLine("Parsing the data\n");

            using StreamReader sr = new StreamReader(_file);
            int index = 0;
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                if (index == 0)
                {
                    _sv.Init(line.Split());
                    _sv.InitCacheServers();
                }
                else
                {
                    _sv.InitVideos(line.Split());
                    break;
                }

                index++;
            }

            for (int i = 0; i < _sv.NumberOfEndpoints; i++)
            {
                line = sr.ReadLine();

                var endpoint = new Endpoint()
                {
                    Id = i,
                    LatencyToDataCenter = Convert.ToInt32(line?.Split()[0]),
                    CacheCount = Convert.ToInt32(line?.Split()[1]),
                };

                int count = endpoint.CacheCount;
                endpoint.CacheServers = new Dictionary<int, int>();
                endpoint.Requests = new Dictionary<int, int>();

                while (count > 0)
                {
                    line = sr.ReadLine();

                    var data = line?.Split().Select(x => Convert.ToInt32(x)).ToList();

                    endpoint.CacheServers.Add(data[0], data[1]);

                    count--;
                }

                _sv.Endpoints.Add(endpoint);
            }

            while ((line = sr.ReadLine()) != null)
            {
                var data = line?.Split().Select(x => Convert.ToInt32(x)).ToList();

                _sv.Endpoints[data[1]].Requests.TryAdd(data[0], data[2]);
            }
        }   

    }
}
