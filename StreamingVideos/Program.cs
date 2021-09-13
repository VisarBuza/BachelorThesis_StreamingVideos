using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamingVideos;
using StreamingVideos.Models;

int time = 3;

try
{
    time = int.Parse(args[0]);
}
catch (Exception e)
{
    Console.WriteLine("No argument given!");
}


var paths = new List<string>
{
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\trending_today.in",
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\me_at_the_zoo.in",
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\videos_worth_spreading.in",
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\kittens.in"
};

var solvers = new List<Solver>
{
    new("trending_today", time),
    new("me_at_the_zoo", time),
    new("videos_worth_spreading", time),
    new("kittens", time)
};

var dataModels = new List<DataModel>();

for (var i = 0; i < 4; i++)
{
    dataModels.Add(new DataModel());
    
    Parser.ParseData(paths[i], dataModels[i]);
}

for (var i = 0; i < solvers.Count; i++)
{
    solvers[i].Init(dataModels[i]);
}

var tasks = solvers.Select(solver => Task.Factory.StartNew(solver.Solve));

await Task.WhenAll(tasks);