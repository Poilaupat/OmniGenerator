using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Image;
using SeedGenerator.Lib.Image.Composer;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Packagers;

namespace SeedGenerator.Cli
{
    internal class Startup
    {
        public static IContainer CreateContainer()
        {
            var config = GetConfiguration();
            var mapper = GetMapper();

            var builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.RegisterType<RootBuilder>().As<IRootBuilder>();
            builder.RegisterInstance(config).As<IConfiguration>();
            builder.RegisterInstance(mapper).As<IMapper>();
            builder.RegisterType<PlainPackager>().As<IPackager>();
            
            builder.RegisterType<ImageComposerProcessor>().As<IImageComposerProcessor>();
            builder.RegisterType<ChequeComposer>()
                .As<IImageComposer>()
                .WithMetadata<IImageComposerMetadata>(m => m.For(icm => icm.DocumentName, "cheque"));
            builder.RegisterType<CouponSepaComposer>().As<IImageComposer>()
                .WithMetadata<IImageComposerMetadata>(m => m.For(icm => icm.DocumentName, "coupon"));


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
