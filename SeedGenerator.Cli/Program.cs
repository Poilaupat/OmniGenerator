// See https://aka.ms/new-console-template for more information
using Autofac;
using SeedGenerator.Cli;
using SeedGenerator.Lib;

string paramFilePath = @"..\..\..\..\ParamFiles\param.json";
string outputFolderPath = @"..\..\..\..\Output";

var container = Startup.CreateContainer();
await container.Resolve<Application>().Run(paramFilePath, outputFolderPath);
