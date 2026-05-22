using Autofac;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Autofac
{
    /// <summary>
    /// <see cref="ITypeResolver"/> implementation for Autofac & Spectre.Cli
    /// </summary>
    public class AutofacTypeResolver : ITypeResolver, IDisposable
    {
        private readonly ILifetimeScope _scope;

        /// <summary>
        /// Constructor that takes a service provider instance.
        /// </summary>
        /// <param name="scope">Scope provider to use in type resolution.</param>
        public AutofacTypeResolver(ILifetimeScope scope)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));
            _scope = scope;
        }

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type, or null if no registration for that
        /// type exists.</returns>
        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                return null;
            }

            return _scope.IsRegistered(type) ? _scope.Resolve(type) : null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_scope is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
