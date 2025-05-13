using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Image;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Autofac
{
    /// <summary>
    /// Autofac module responsible for registering application-level services,
    /// infrastructure components, and utilities required by the OmniGenerator library.
    /// </summary>
    internal class OmniGeneratorModule : Module
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="OmniGeneratorModule"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration, used for Serilog setup and potentially other components.</param>
        public OmniGeneratorModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Loads the module and registers types into the Autofac container.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register Serilog logger into the Autofac container using extension method
            builder.AddSerilog(_configuration);

            // Register AutoMapper instance with scanned profiles from the executing assembly
            builder.RegisterInstance(GetMapper()).As<IMapper>().SingleInstance();

            // Register core OmniGenerator services and interfaces
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentDrawerManager>().As<IDocumentDrawerManager>().InstancePerLifetimeScope();
            builder.RegisterType<PluginService>().As<IPluginService>().InstancePerLifetimeScope();

            // Register open generic type for progress reporting
            builder.RegisterGeneric(typeof(Progress<>)).As(typeof(IProgress<>)).InstancePerLifetimeScope();

            // Register concrete progress report type
            builder.RegisterType<HierarchyBuilderProgress>().InstancePerDependency();
        }

        /// <summary>
        /// Creates and configures the AutoMapper instance used throughout the application.
        /// Automatically scans the current assembly for profile definitions.
        /// </summary>
        /// <returns>An initialized <see cref="IMapper"/> instance.</returns>
        private static IMapper GetMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                // Automatically scan current assembly for AutoMapper profiles
                cfg.AddMaps(System.Reflection.Assembly.GetExecutingAssembly());
            }).CreateMapper();
        }
    }
}
