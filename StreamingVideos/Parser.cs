using System;
using System.IO;
using StreamingVideos.Models;
using System.Linq;

namespace StreamingVideos
    {
    public static class Parser
    {
        public static void ParseData(string file, StreamingVideoService streamingVideoService)
        {
            Console.WriteLine("Parsing the data\n");

            using var sr = new StreamReader(file);
            var line = sr.ReadLine();
            streamingVideoService.Init(line.Split());

            line = sr.ReadLine();
            streamingVideoService.InitVideos(line.Split());

            for (var i = 0; i < streamingVideoService.NumberOfEndpoints; i++)
            {
                line = sr.ReadLine();

                var endpoint = new Endpoint
                {
                    Id = i,
                    LatencyToDataCenter = int.Parse(line.Split()[0]),
                    CacheCount = int.Parse(line.Split()[1]),
                };

                for (var j = 0; j < endpoint.CacheCount; j++)
                {
                    line = sr.ReadLine();
                    var data = line.Split().Select(int.Parse).ToList();
                    endpoint.CacheServers.Add(data[0], data[1]);
                }

                streamingVideoService.Endpoints.Add(endpoint);
            }

            while ((line = sr.ReadLine()) != null)
            {
                var requestData = line.Split().Select(int.Parse).ToList();
                var request = new Request
                {
                    Video = requestData[0], 
                    Endpoint = requestData[1], 
                    RequestNo = requestData[2]
                };
                
                streamingVideoService.Requests.Add(request);
                streamingVideoService.Endpoints[request.Endpoint].Requests[request.Video] = request.RequestNo;
            }
        }
    }
}
