using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OmniGenerator.Cli.Autofac
{
    /// <summary>
    /// Autofac module responsible for registering configuration-related services.
    /// This includes:
    /// - Registering the <see cref="IConfiguration"/> instance for injection
    /// - Registering strongly-typed configuration settings (AppSettings) using the Microsoft options pattern
    /// </summary>
    internal sealed class ConfigurationModule : Module
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationModule"/> class.
        /// </summary>
        /// <param name="configuration">The application's configuration root (e.g., loaded from appsettings.json)</param>
        public ConfigurationModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Override of the Autofac <see cref="Module.Load"/> method.
        /// Performs all configuration-related registrations into the container.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register the IConfiguration instance so it can be injected wherever needed.
            builder.Register(context => _configuration).As<IConfiguration>().SingleInstance();

            // Register strongly-typed configuration section (AppSettings) using Microsoft.Extensions.Options pattern
            var services = new ServiceCollection();
            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            // Populate the Autofac container with Microsoft.Extensions.DependencyInjection services
            builder.Populate(services);
        }
    }
}
