using System.Composition.Hosting;
using System.Reflection;

public class PluginLoader
{
    public CompositionHost LoadPlugins(string pluginsDirectory)
    {
        var assemblies = new List<Assembly>();

        foreach (var file in Directory.GetFiles(pluginsDirectory, "*.dll"))
        {
            var pluginLoadContext = new PluginLoadContext(file);
            var assembly = pluginLoadContext.LoadFromAssemblyPath(file);
            assemblies.Add(assembly);
        }

        var configuration = new ContainerConfiguration()
            .WithAssemblies(assemblies);

        return configuration.CreateContainer();
    }
}