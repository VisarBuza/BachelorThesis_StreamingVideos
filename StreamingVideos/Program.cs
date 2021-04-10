using StreamingVideos;

const string path = @"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\example.in";

var streamingVideoService = new StreamingVideoService();

Parser.ParseData(path, streamingVideoService);

streamingVideoService.Compute();
streamingVideoService.LogData();