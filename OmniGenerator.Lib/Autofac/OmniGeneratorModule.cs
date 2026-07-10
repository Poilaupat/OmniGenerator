using Autofac;
using DotLiquid;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Renderers;
using OmniGenerator.Lib.ErrorSimulation;
using OmniGenerator.Lib.ErrorSimulation.Mutators;
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
            // Register DotLiquid custom filters
            Template.RegisterFilter(typeof(LiquidCustomFilters));

            // Register Serilog logger into the Autofac container using extension method
            builder.AddSerilog(_configuration);
            builder.RegisterType<FieldMapper>().As<IFieldMapper>().SingleInstance();

            // Register core OmniGenerator services and interfaces
            var maxParallelism = _configuration.GetValue<int>("AppSettings:max-parallelism", -1);
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>()
                .WithParameter("maxParallelism", maxParallelism)
                .InstancePerLifetimeScope();
            builder.RegisterType<DocumentRendererManager>().As<IDocumentRendererManager>().InstancePerLifetimeScope();
            builder.RegisterType<PhysicalFileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<PluginService>().As<IPluginService>().InstancePerLifetimeScope();
            builder.RegisterType<GenerationOrchestrator>().As<IGenerationOrchestrator>().InstancePerLifetimeScope();

            // Register error simulation engine and its channel-specific mutators
            builder.RegisterType<MisreadMutator>().As<IFieldErrorMutator>().SingleInstance();
            builder.RegisterType<SubstitutionMutator>().As<IFieldErrorMutator>().SingleInstance();
            builder.RegisterType<InconsistencyMutator>().As<IFieldErrorMutator>().SingleInstance();
            builder.RegisterType<ErrorSimulator>().As<IErrorSimulator>().InstancePerLifetimeScope();

            // Register open generic progress hub (singleton: stores last known state per jobId)
            builder.RegisterGeneric(typeof(ProgressHub<>))
                .As(typeof(IProgressHub<>))
                .SingleInstance();
        }
    }
}
