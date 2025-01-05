using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Image;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Cli
{
    internal class Startup
    {
        public static IContainer CreateContainer()
        {
            var config = GetConfiguration();
            var mapper = GetMapper();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(mapper).As<IMapper>();
            builder.RegisterType<Application>();
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>();
            builder.RegisterType<DocumentDrawerManager>().As<IDocumentDrawerManager>();
            builder.RegisterType<PluginService>().As<IPluginService>();

            builder.RegisterGeneric(typeof(Progress<>)).As(typeof(IProgress<>)).InstancePerLifetimeScope();
            builder.RegisterType<BuilderProgressReport>();

            //Configuration
            builder.RegisterInstance(config).As<IConfiguration>();
            

            return builder.Build();
        }

        private static IConfiguration GetConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
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
