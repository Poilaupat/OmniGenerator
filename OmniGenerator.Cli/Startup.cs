using Autofac;
using AutoMapper;
using BenchmarkDotNet.Columns;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Image;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Tools;
using CommandLine;
using OmniGenerator.Cli.Options;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace OmniGenerator.Cli
{
    internal static class Startup
    {
        public static void AddOmniGeneratorCliDependencies(this ContainerBuilder builder, ICommandLineOptions options, IConfiguration configuration)
        {
            //Command line options registration
            builder.RegisterInstance(options).As<ICommandLineOptions>();

            //Logger registration
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);

            builder.RegisterSerilog(loggerConfiguration);

            //AutoMapper instance registration
            var mapper = GetMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            //OmniGenerator types registration
            builder.RegisterType<OmniGeneratorCliApplication>();
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>();
            builder.RegisterType<DocumentDrawerManager>().As<IDocumentDrawerManager>();
            builder.RegisterType<PluginService>().As<IPluginService>();

            //Open generic type for IProgress registration followed by progress report concrete types registration
            builder.RegisterGeneric(typeof(Progress<>)).As(typeof(IProgress<>)).InstancePerLifetimeScope();
            builder.RegisterType<HierarchyBuilderProgressReport>();
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new[] { "OmniGenerator.Lib" });
            }).CreateMapper();
        }
    }
}
