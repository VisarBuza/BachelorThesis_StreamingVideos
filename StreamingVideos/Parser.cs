using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StreamingVideos.Models;
using System.Linq;

namespace StreamingVideos
    {
    public class Parser
    {
        private readonly string _file;
        private readonly StreamingVideoService _streamingVideoServiceService;

        public Parser(string file, StreamingVideoService streamingVideoServiceService)
        {
            _file = file;
            _streamingVideoServiceService = streamingVideoServiceService;
        }
        
        public void ParseData()
        {
            Console.WriteLine("Parsing the data\n");

            using var sr = new StreamReader(_file);
            
            var line = sr.ReadLine();
            _streamingVideoServiceService.Init(line.Split());

            line = sr.ReadLine();
            _streamingVideoServiceService.InitVideos(line.Split());

            for (var i = 0; i < _streamingVideoServiceService.NumberOfEndpoints; i++)
            {
                line = sr.ReadLine();

                var endpoint = new Endpoint
                {
                    Id = i,
                    LatencyToDataCenter = Convert.ToInt32(line?.Split()[0]),
                    CacheCount = Convert.ToInt32(line?.Split()[1]),
                };

                var count = endpoint.CacheCount;
                endpoint.CacheServers = new Dictionary<int, int>();
                endpoint.Requests = new Dictionary<int, int>();

                while (count > 0)
                {
                    line = sr.ReadLine();

                    var data = line?.Split().Select(int.Parse).ToList();

                    endpoint.CacheServers.Add(data[0], data[1]);

                    count--;
                }

                _streamingVideoServiceService.Endpoints.Add(endpoint);
            }

            while ((line = sr.ReadLine()) != null)
            {
                var data = line?.Split().Select(int.Parse).ToList();

                _streamingVideoServiceService.Endpoints[data[1]].Requests.Add(data[0], data[2]);
            }
        }   

    }
}
