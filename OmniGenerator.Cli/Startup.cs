using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Image;
using OmniGenerator.Lib.Image.Composer;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Packagers.Compliance;
using OmniGenerator.Lib.Packagers.Default;

namespace OmniGenerator.Cli
{
    internal class Startup
    {
        public static IContainer CreateContainer()
        {
            var config = GetConfiguration();
            var mapper = GetMapper();

            var builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>();
            builder.RegisterInstance(config).As<IConfiguration>();
            builder.RegisterInstance(mapper).As<IMapper>();
            //builder.RegisterType<PlainPackager>().As<IPackager>();
            builder.RegisterType<EligibilityPackager>().As<IPackager>();

            builder.RegisterType<DefaultImageComposerProcessor>().As<IImageComposerProcessor>();
            builder.RegisterType<ChequeComposer>()
                .As<IImageComposer>()
                .WithMetadata<IImageComposerMetadata>(m => m.For(icm => icm.DocumentName, "cheque"));
            builder.RegisterType<TalonSepaComposer>().As<IImageComposer>()
                .WithMetadata<IImageComposerMetadata>(m => m.For(icm => icm.DocumentName, "talon-optique"));


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
