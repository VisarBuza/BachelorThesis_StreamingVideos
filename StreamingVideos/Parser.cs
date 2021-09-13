using System;
using System.IO;
using StreamingVideos.Models;
using System.Linq;

namespace StreamingVideos
{
    public static class Parser
    {
        public static DataModel ParseData(string file, DataModel dataModel)
        {
            Console.WriteLine($"Parsing {file.Split("\\").Last()}\n");

            using var sr = new StreamReader(file);

            ParseFirstLine(sr, dataModel);
            ParseVideos(sr, dataModel);
            ParseEndpoints(sr, dataModel);
            ParseRequests(sr, dataModel);

            Console.WriteLine($"Finished parsing the data\n");

            return dataModel;
        }

        private static void ParseFirstLine(StreamReader sr, DataModel dataModel)
        {
            var line = sr.ReadLine();
            var data = line!.Split();

            dataModel.NumVideos = int.Parse(data[0]);
            dataModel.NumEndpoints = int.Parse(data[1]);
            dataModel.NumRequests = int.Parse(data[2]);
            dataModel.NumCaches = int.Parse(data[3]);
            dataModel.CacheCapacity = int.Parse(data[4]);
        }

        private static void ParseVideos(StreamReader sr, DataModel dataModel)
        {
            var line = sr.ReadLine();
            var data = line!.Split();
            dataModel.VideoSizes.AddRange(data.Select(int.Parse));
        }

        private static void ParseEndpoints(StreamReader sr, DataModel dataModel)
        {
            for (var i = 0; i < dataModel.NumEndpoints; i++)
            {
                var line = sr.ReadLine();

                var endpoint = new Endpoint
                {
                    Id = i,
                    LatencyToDataCenter = int.Parse(line!.Split()[0]),
                    CacheCount = int.Parse(line.Split()[1]),
                };

                for (var j = 0; j < endpoint.CacheCount; j++)
                {
                    line = sr.ReadLine();
                    var data = line!.Split().Select(int.Parse).ToList();
                    endpoint.CacheServers.Add(data[0], data[1]);
                }

                dataModel.Endpoints.Add(endpoint);
            }
        }

        private static void ParseRequests(StreamReader sr, DataModel dataModel)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var requestData = line.Split().Select(int.Parse).ToList();
                var request = new Request
                {
                    Video = requestData[0],
                    Endpoint = requestData[1],
                    RequestNo = requestData[2],
                    LowestLatency = dataModel.Endpoints[requestData[1]].LatencyToDataCenter
                };

                dataModel.Requests.Add(request);
            }
        }
    }
}
