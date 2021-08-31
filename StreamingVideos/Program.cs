using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamingVideos;
using StreamingVideos.Models;

var paths = new List<string>
{
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\trending_today.in",
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\me_at_the_zoo.in",
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\videos_worth_spreading.in",
    @$"C:\Programming\BachelorThesis_StreamingVideos\StreamingVideos\Dataset\kittens.in"
};

var solvers = new List<Solver>
{
    new("trending_today.in"),
    new("me_at_the_zoo.in"),
    new("videos_worth_spreading.in"),
    new("kittens.in")
};

var dataModels = new List<DataModel>();

for (var i = 0; i < 4; i++)
{
    dataModels.Add(new DataModel());
    
    Parser.ParseData(paths[i], dataModels[i]);

    solvers[i].Init(dataModels[i]);
}

var tasks = solvers.Select(solver => Task.Factory.StartNew(solver.Solve));

await Task.WhenAll(tasks);