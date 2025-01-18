using Autofac;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli
{
    public class BenchmarkHelper
    {
        private readonly string _configFilePath = "C:\\Users\\Ruben\\source\\repos\\OmniGenerator\\ParamFiles\\param-compliance-eligibility.json";
        private IContainer? _container;
        private OmniGeneratorConfiguration? _config;

        //[Params(1, 10, 100, 1000, 5000)]
        //public int PartitionSize { get; set; }

        [GlobalSetup]
        public async Task SetupAsync()
        {
            _config = await ConfigurationReader.ReadConfigurationAsync(_configFilePath);
            ConfigurationReader.CheckConfiguration(_config);

            _container = Startup.CreateContainer();
        }

        [Benchmark(Baseline = true)]
        public async Task BuildUsingHierarchyBuilder()
        {
            var mapper = _container!.Resolve<IMapper>();
            var appconfig = _container!.Resolve<IConfiguration>();

            HierarchyBuilder builder = new(mapper, appconfig);
            await builder.BuildAsync(_config!, null);
        }

        [Benchmark]
        public async Task BuildUsingHierarchyBuilderAlt()
        {
            int i = 0;

            var mapper = _container!.Resolve<IMapper>();
            var appconfig = _container!.Resolve<IConfiguration>();

            HierarchyBuilder builder = new(mapper, appconfig);
            await builder.BuildAsync(_config!, new Progress<HierarchyBuilderProgressReport>(pr =>
            {
                i++;
            }));
        }
    }
}
