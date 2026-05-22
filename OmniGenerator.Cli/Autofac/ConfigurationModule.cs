using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmniGenerator.Cli.Quartz;
using Quartz;

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
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            _configuration = configuration;
        }

        /// <summary>
        /// Override of the Autofac <see cref="Module.Load"/> method.
        /// Performs all configuration-related registrations into the container.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register the IConfiguration instance so it can be injected wherever needed.
            builder.Register(_ => _configuration).As<IConfiguration>().SingleInstance();

            // Register strongly-typed configuration section (AppSettings) using Microsoft.Extensions.Options pattern
            var services = new ServiceCollection();
            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            // Quartz.NET registration (so Quartz services can be injected via Autofac)
            services.AddQuartz(static q =>
            {
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
            });
            services.AddQuartzHostedService(static o =>
            {
                o.WaitForJobsToComplete = true;
            });

            // Populate the Autofac container with Microsoft.Extensions.DependencyInjection services
            builder.Populate(services);

            // Register JobStateListener as a singleton so it is shared between GenerateManyCommand and the Quartz listener registration
            builder.RegisterType<JobStateListener>().AsSelf().SingleInstance();
        }
    }
}
