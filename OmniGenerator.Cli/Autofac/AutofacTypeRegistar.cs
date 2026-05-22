using Autofac;
using Spectre.Console.Cli;
using System;

namespace OmniGenerator.Cli.Autofac
{
    /// <summary>
    /// Provides an Autofac-based implementation of <see cref="ITypeRegistrar"/> for integrating
    /// Autofac with Spectre.Console.Cli dependency injection.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AutofacTypeRegistrar"/> class
    /// using the specified <see cref="ContainerBuilder"/>.
    /// </remarks>
    /// <param name="builder">The Autofac <see cref="ContainerBuilder"/> used for service registration.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <c>null</c>.</exception>
    public sealed class AutofacTypeRegistrar(ContainerBuilder builder) : ITypeRegistrar
    {
        private readonly ContainerBuilder _builder = builder ?? throw new ArgumentNullException(nameof(builder));

        /// <summary>
        /// Registers a service type and its corresponding implementation type in the Autofac container.
        /// </summary>
        /// <param name="service">The interface or base type to register.</param>
        /// <param name="implementation">The concrete type that implements the <paramref name="service"/>.</param>
        public void Register(Type service, Type implementation)
        {
            _builder.RegisterType(implementation).As(service);
        }

        /// <summary>
        /// Registers a specific instance as a service in the Autofac container.
        /// </summary>
        /// <param name="service">The interface or base type to register the instance as.</param>
        /// <param name="implementation">The actual instance to register.</param>
        public void RegisterInstance(Type service, object implementation)
        {
            _builder.RegisterInstance(implementation).As(service);
        }

        /// <summary>
        /// Registers a lazy factory method that provides an instance of a given service type.
        /// </summary>
        /// <param name="service">The service type to register.</param>
        /// <param name="factory">A factory method that returns an instance of the service.</param>
        public void RegisterLazy(Type service, Func<object> factory)
        {
            _builder.Register(_ => factory()).As(service);
        }

        /// <summary>
        /// Finalizes the container registration and builds a type resolver
        /// that can be used by Spectre.Console.Cli to resolve dependencies.
        /// </summary>
        /// <returns>A new instance of <see cref="ITypeResolver"/> backed by an Autofac container.</returns>
        public ITypeResolver Build()
        {
            var container = _builder.Build();
            return new AutofacTypeResolver(container);
        }
    }
}
