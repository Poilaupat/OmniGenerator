// See https://aka.ms/new-console-template for more information
using Autofac;
using BenchmarkDotNet.Running;
using OmniGenerator.Cli;
using OmniGenerator.Lib.Generators;

string configFilePath = @"..\..\..\..\ParamFiles\param.json";
//string configFilePath = @"..\..\..\..\ParamFiles\param-sql.json";
//string configFilePath = @"..\..\..\..\ParamFiles\param-compliance-eligibility.json";
string outputFolderPath = @"..\..\..\..\Output";

var container = Startup.CreateContainer();
await container.Resolve<Application>().Run(configFilePath, outputFolderPath);

//BenchmarkRunner.Run<BenchmarkHelper>();