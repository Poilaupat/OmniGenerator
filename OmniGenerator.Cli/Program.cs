// See https://aka.ms/new-console-template for more information
using Autofac;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Cli.Autofac;
using OmniGenerator.Cli.Commands;
using OmniGenerator.Lib.Autofac;
using Serilog;
using Spectre.Console.Cli;


//Configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//Dependency injection registar (Spectre needs a registrar not a container)
var builder = new ContainerBuilder();
builder.RegisterModule(new OmniGeneratorModule(configuration));
var registrar = new AutofacTypeRegistrar(builder);


try
{
    //Spectre.Cli configuration
    var app = new CommandApp(registrar);
    app.Configure(commands =>
    {
        commands.AddCommand<VersionCommand>("version");
        commands.AddCommand<GenerateCommand>("generate");
    });
    //Spectre.Cli app run
    await app.RunAsync(args); 
}
catch (Exception e)
{
    Log.Error(e, "Error");
}
finally
{
    await Log.CloseAndFlushAsync();
}




