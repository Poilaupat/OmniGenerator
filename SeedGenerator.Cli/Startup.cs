using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedGenerator.Lib;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Plugins.Image.Composers;
using SeedGenerator.Plugins.Packagers;

namespace SeedGenerator.Cli
{
    internal class Startup
    {
        public static IConfiguration GetConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }


        public static IServiceCollection ConfigureServices(IConfiguration configuration)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IMapper>(_ =>
                new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(new[] { "SeedGenerator.Lib" });
                }).CreateMapper()
            );
            services.AddTransient<SeedBuilder>();
            services.AddTransient<IPackager, ZipPackager>();
            services.AddTransient<IImageComposer, ChequeComposer>();
            return services;
        }
    }
}
