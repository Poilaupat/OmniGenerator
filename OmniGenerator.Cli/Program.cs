// See https://aka.ms/new-console-template for more information
using Autofac;
using OmniGenerator.Cli;
using OmniGenerator.Lib;

string configFilePath = @"..\..\..\..\ParamFiles\param-compliance-eligibility.json";
string outputFolderPath = @"..\..\..\..\Output";

var container = Startup.CreateContainer();
await container.Resolve<Application>().Run(configFilePath, outputFolderPath);
