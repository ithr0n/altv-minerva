using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PlayGermany.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }

        private static readonly List<Type> servicesToInstanciate = new List<Type>();

        public static void AddScopedAndInstanciate<T>(this ServiceCollection services)
            where T : class
        {
            services.AddScoped<T>();
            servicesToInstanciate.Add(typeof(T));
        }

        public static void AddSingletonAndInstanciate<T>(this ServiceCollection services)
            where T : class
        {
            services.AddSingleton<T>();
            servicesToInstanciate.Add(typeof(T));
        }

        public static void InstanciateRegisteredServices(this ServiceProvider provider)
        {
            foreach (var type in servicesToInstanciate)
            {
                _ = provider.GetService(type);
            }
        }
    }
}
