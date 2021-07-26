using System;
using StreamingVideos;
using StreamingVideos.Models;

var path = @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\{Environment.GetCommandLineArgs()[1]}";

var dataModel = new DataModel();

Parser.ParseData(path, dataModel);

var solver = new Solver();

solver.Init(dataModel);

solver.Solve();