// See https://aka.ms/new-console-template for more information
using Autofac;
using SeedGenerator.Cli;
using SeedGenerator.Lib;

string paramFilePath = @"C:\Users\RDE\source\repos\Poilaupat\SeedGenerator\ParamFiles\param.json";
string outputFolderPath = @"D:\Work\6 - Autres projets\SeedGenerator\Output";

var container = Startup.CreateContainer();
await container.Resolve<SeedBuilderApplication>().Run(paramFilePath, outputFolderPath);
