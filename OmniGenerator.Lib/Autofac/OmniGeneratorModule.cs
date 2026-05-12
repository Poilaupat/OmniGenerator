using Autofac;
using DotLiquid;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Renderers;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Liquid;
using OmniGenerator.Lib.Mapping;
using OmniGenerator.Lib.Orchestration;
using OmniGenerator.Lib.Hierarchy.Interfaces;
using OmniGenerator.Lib.Mapping.Interfaces;
using OmniGenerator.Lib.Orchestration.Interfaces;
using OmniGenerator.Lib.Renderers.Interfaces;
using OmniGenerator.Lib.Reporting;

namespace OmniGenerator.Lib.Autofac
{
    /// <summary>
    /// Autofac module responsible for registering application-level services,
    /// infrastructure components, and utilities required by the OmniGenerator library.
    /// </summary>
    internal class OmniGeneratorModule : Module
    {
        private readonly IConfiguration _configuration;
        private static bool _liquidFiltersRegistered = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="OmniGeneratorModule"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration, used for Serilog setup and potentially other components.</param>
        public OmniGeneratorModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Register DotLiquid custom filters once
            RegisterLiquidFilters();
        }

        /// <summary>
        /// Registers custom DotLiquid filters for use in composite field templates.
        /// This method is called once during module initialization.
        /// </summary>
        private static void RegisterLiquidFilters()
        {
            if (_liquidFiltersRegistered) return;

            Template.RegisterFilter(typeof(LiquidCustomFilters));
            _liquidFiltersRegistered = true;
        }

        /// <summary>
        /// Loads the module and registers types into the Autofac container.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register Serilog logger into the Autofac container using extension method
            builder.AddSerilog(_configuration);

            // Register Mapperly mapper instance
            builder.RegisterType<FieldMapper>().As<IFieldMapper>().SingleInstance();

            // Register core OmniGenerator services and interfaces
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentRendererManager>().As<IDocumentRendererManager>().InstancePerLifetimeScope();
            builder.RegisterType<PluginService>().As<IPluginService>().InstancePerLifetimeScope();
            builder.RegisterType<GenerationOrchestrator>().As<IGenerationOrchestrator>().InstancePerLifetimeScope();

            // Register open generic progress hub (singleton: stores last known state per jobId)
            builder.RegisterGeneric(typeof(ProgressHub<>))
                .As(typeof(IProgressHub<>))
                .SingleInstance();
        }
    }
}
