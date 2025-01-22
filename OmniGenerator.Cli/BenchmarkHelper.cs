using Autofac;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli
{
    //public class BenchmarkHelper
    //{
    //    private OmniGeneratorConfiguration? _config;
    //    private Microsoft.Extensions.Hosting.IHost? _host;

    //    //[Params(1, 10, 100, 1000, 5000)]
    //    //public int PartitionSize { get; set; }

    //    [GlobalSetup]
    //    public async Task SetupAsync()
    //    {
    //        _host = HostBuilder.Get();

    //        _config = await ConfigurationReader.ReadConfigurationAsync(args[1]);
    //        ConfigurationReader.CheckConfiguration(_config);
    //    }

    //    [Benchmark(Baseline = true)]
    //    public async Task BuildUsingHierarchyBuilder()
    //    {
    //        var mapper = _host!.Services.GetRequiredService<IMapper>();
    //        var appconfig = _host!.Services.GetRequiredService<IConfiguration>();

    //        HierarchyBuilder builder = new(mapper, appconfig);
    //        await builder.BuildAsync(_config!, null);
    //    }

    //    [Benchmark]
    //    public async Task BuildUsingHierarchyBuilderAlt()
    //    {
    //        int i = 0;

    //        var mapper = _host!.Services.GetRequiredService<IMapper>();
    //        var appconfig = _host!.Services.GetRequiredService<IConfiguration>();

    //        HierarchyBuilder builder = new(mapper, appconfig);
    //        await builder.BuildAsync(_config!, new Progress<HierarchyBuilderProgressReport>(pr =>
    //        {
    //            i++;
    //        }));
    //    }
    //}
}
