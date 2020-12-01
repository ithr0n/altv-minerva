using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PlayGermany.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesOfInterface = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T)));
            foreach (var type in typesOfInterface)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }

            var typesOfClasses = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T)));
            foreach (var type in typesOfClasses)
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
