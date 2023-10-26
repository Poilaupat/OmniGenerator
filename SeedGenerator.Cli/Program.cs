// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedGenerator.Cli;
using SeedGenerator.Lib;


IConfiguration configuration = Startup.GetConfiguration();
IServiceCollection services = Startup.ConfigureServices(configuration);
ServiceProvider provider = services.BuildServiceProvider();

await provider.GetRequiredService<Application>().Run(args);
