using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Image;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog.Extensions.Autofac.DependencyInjection;
using Serilog;
using Autofac.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace OmniGenerator.Lib.Autofac
{
    internal class OmniGeneratorModule : Module
    {
        private IConfiguration _configuration;

        public OmniGeneratorModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //Configuration registration
            builder.Register(context => _configuration).As<IConfiguration>();

            //Logger registration
            builder.AddSerilog(_configuration);

            //AutoMapper registration
            builder.RegisterInstance(GetMapper()).As<IMapper>();

            //OmniGenerator types registration
            builder.RegisterType<HierarchyBuilder>().As<IHierarchyBuilder>();
            builder.RegisterType<DocumentDrawerManager>().As<IDocumentDrawerManager>();
            builder.RegisterType<PluginService>().As<IPluginService>();

            //Open generic type for IProgress registration followed by progress report concrete types registration
            builder.RegisterGeneric(typeof(Progress<>)).As(typeof(IProgress<>)).InstancePerLifetimeScope();
            builder.RegisterType<HierarchyBuilderProgressReport>();
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(System.Reflection.Assembly.GetExecutingAssembly());
            }).CreateMapper();
        }
    }
}
