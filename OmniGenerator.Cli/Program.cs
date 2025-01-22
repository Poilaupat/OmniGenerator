// See https://aka.ms/new-console-template for more information
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmniGenerator.Cli;
using CommandLine;
using OmniGenerator.Cli.Options;
using Microsoft.Extensions.Configuration;
using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

await Parser
    .Default
    .ParseArguments<DefaultOptions>(args)
    .WithParsedAsync(async options =>
    {
        // Creating an application host. Note that the host will never be started. 
        // We use it bc it's a convenient way to use DI and other usefull features in a console application
        var host = Host
            .CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices((context, services) =>
            {
                // Services configured in Microsoft DI container (easier to register configuration with a service collection)
                // Thoses types will be passed to Autofac container automatically
                services.Configure<ApplicationSettings>(configuration.GetSection("general-settings"));

            })
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                // Services configured directly in Autofac container
                builder.AddOmniGeneratorCliDependencies(options, configuration);
            })
            .Build();

        // Emulating the behavior of a real running host concerning application closing
        CancellationTokenSource cts = new CancellationTokenSource();
        bool properlyClose = true;
        Console.CancelKeyPress += new ConsoleCancelEventHandler((_, cancelEventHandler) =>
        {
            if (properlyClose) // First Ctrl+C
            {
                Console.WriteLine("Cancellelation signal received. Starting OmniGenerator shutdown.");
                cts.Cancel();
                cancelEventHandler.Cancel = true;
                properlyClose = false;

                // Letting a chance to the application to shutdown properly during 5 seconds
                var timer = new System.Timers.Timer(5000);
                timer.Elapsed += (_, ElapsedEventArgs) =>
                {
                    Console.WriteLine("OmniGenerator is not responding. Forcing shutdown.");
                    Environment.Exit(0);
                };
                timer.Start();
            }
            else // Second Ctrl+C
            {
                Console.WriteLine("Force closing signal received. Forcing shutdown.");
                Environment.Exit(0);
            }
        });

        // Application start
        try
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                var cliApp = scope.ServiceProvider.GetRequiredService<OmniGeneratorCliApplication>();
                await cliApp.RunAsync();
            }
        }
        finally
        {
            Log.Logger.Debug("Fuck you bitch");

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError("Fuck you too bitch!");

            var ex = new ArgumentException(nameof(logger));
            logger.LogCritical(ex, "Couille dans le paté. Je répète. Couille dans le paté");
        
            await Log.CloseAndFlushAsync();
        }
    });




