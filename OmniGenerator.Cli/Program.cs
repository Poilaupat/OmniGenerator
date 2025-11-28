// See https://aka.ms/new-console-template for more information
using Autofac;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Cli.Autofac;
using OmniGenerator.Cli.Commands;
using OmniGenerator.Lib.Autofac;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;

//Configuration file
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//Dependency injection registar (Spectre needs a registrar not a container)
var builder = new ContainerBuilder();
builder.RegisterModule(new ConfigurationModule(configuration));
builder.RegisterModule(new OmniGeneratorModule(configuration));
var registrar = new AutofacTypeRegistrar(builder);

// Managing clean process stop
using var cts = new CancellationTokenSource();
var forceCloseAppOnNextCancel = false;
Console.CancelKeyPress += (sender, e) =>
{
    if (!forceCloseAppOnNextCancel)
    {
        e.Cancel = true; //Cancelling event to let some time to commands to end gracefully
        AnsiConsole.MarkupLine("[yellow]User cancellation requested. Trying to close app gracefully[/]");
        cts.Cancel();
        forceCloseAppOnNextCancel = true;
    }
    else
    {
        e.Cancel = false; // Lets event propagate to system in order to force app close
        AnsiConsole.MarkupLine("[red]User force cancellation requested. Closing app[/]");
        Environment.Exit(2);
    }
};

try
{
    //Spectre.Cli configuration
    var app = new CommandApp(registrar);
    app.Configure(commands =>
    {
        commands.AddCommand<AboutCommand>("about")
            .WithDescription("Provides info about OmniGenerator")
            .WithExample("about");

        commands.AddCommand<GenerateCommand>("generate");

        commands.AddBranch<PluginCommandSettingsBase>("plugin", plugin =>
        {
            plugin.AddCommand<PluginListCommand>("list")
                .WithDescription("List plugins installed")
                .WithExample("plugin", "list", "--packagers");

            plugin.AddCommand<PluginDetailCommand>("details")
                .WithDescription("Displays details on a specifi plugin")
                .WithExample("plugin", "details", "\"PLUGIN_NAME\"");
        });

#if DEBUG
        commands.AddCommand<InfiniteCommand>("infinite")
        .WithDescription("An command that takes an infinite amount of time to execute. Usefull to test CancellableAsyncCommand.")
        .WithExample("infinite")
        .WithExample("infinite", "--cancellable");
#endif
    });

    //Spectre.Cli app run
    await app.RunAsync(args, cts.Token);
}
catch (Exception e)
{
    Log.Error(e, "Error"); //TODO : Do not seems to log ??
}
finally
{
    await Log.CloseAndFlushAsync();
}