using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace OmniGenerator.Lib.Autofac
{
    /// <summary>
    /// Provides an extension method to register and configure Serilog logging
    /// within an Autofac <see cref="ContainerBuilder"/> using configuration from <see cref="IConfiguration"/>.
    /// </summary>
    internal static class SerilogDependencyInjection
    {
        /// <summary>
        /// Registers Serilog as the logging provider in the Autofac container using
        /// the specified application <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">The Autofac <see cref="ContainerBuilder"/> to register services with.</param>
        /// <param name="configuration">The application configuration used to configure Serilog.</param>
        public static void AddSerilog(this ContainerBuilder builder, IConfiguration configuration)
        {
            // Create the Serilog logger from configuration
            var serilog = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration)
                .CreateLogger();

            // Create a service collection and configure logging with Serilog
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(serilog, dispose: true);
            });

            // Populate the Autofac container with the configured services
            builder.Populate(services);
        }
    }
}
