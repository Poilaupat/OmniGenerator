// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedGenerator.Cli;
using SeedGenerator.Lib;

IConfiguration configuration = Startup.GetConfiguration();
IServiceCollection services = Startup.ConfigureServices(configuration);
ServiceProvider provider = services.BuildServiceProvider();

var builder = provider.GetRequiredService<SeedBuilder>();

await builder.LoadParam(@"C:\Users\Ruben\source\repos\SeedGenerator\ParamFiles\param.json");
var packetData = builder.BuildSeed(@"C:\Users\Ruben\source\repos\SeedGenerator\Output");
;
