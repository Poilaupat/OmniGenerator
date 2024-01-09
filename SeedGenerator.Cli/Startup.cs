using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Plugins.Image.Composers;
using SeedGenerator.Plugins.Packagers;
using static System.Net.Mime.MediaTypeNames;

namespace SeedGenerator.Cli
{
    internal class Startup
    {
        public static IContainer CreateContainer()
        {
            var config = GetConfiguration();
            var mapper = GetMapper();

            var builder = new ContainerBuilder();
            builder.RegisterType<SeedBuilderApplication>();
            builder.RegisterInstance(config).As<IConfiguration>();
            builder.RegisterInstance(mapper).As<IMapper>();
            builder.RegisterType<PlainPackager>().As<IPackager>();
            builder.RegisterType<ChequeComposer>().As<IImageComposer>();
            builder.RegisterType<ChequeRedComposer>().As<IImageComposer>();
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
                cfg.AddMaps(new[] { "SeedGenerator.Lib" });
            }).CreateMapper();
        }
    }
}
