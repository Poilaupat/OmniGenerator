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

namespace OmniGenerator.Cli
{
    internal static class Startup
    {
        public static void AddOmniGeneratorCliDependencies(this ContainerBuilder builder, ICommandLineOptions options)
        {
            //Command line options registering
            builder.RegisterInstance(options).As<ICommandLineOptions>();

            //Configuration registering
            //builder.RegisterInstance(configuration).As<IConfiguration>();

            //AutoMapper instance registering
            var mapper = GetMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            //OmniGenerator types
            builder.RegisterType<OmniGeneratorCliApplication>();
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>();
            builder.RegisterType<DocumentDrawerManager>().As<IDocumentDrawerManager>();
            builder.RegisterType<PluginService>().As<IPluginService>();

            //Open generic type for IProgress followed by progress report concrete types
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
