using Microsoft.Extensions.DependencyInjection;
using Minerva.Server.Contracts.ScriptStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Minerva.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where T : class
        {
            var typesOfInterface = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T)));
            foreach (var type in typesOfInterface)
            {
                if (type.ImplementedInterfaces.Contains(typeof(IStartupSingletonScript)))
                {
                    servicesToInstanciate.Add(type);
                }

                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }

            var typesOfClasses = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T)));
            foreach (var type in typesOfClasses)
            {
                if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IStartupSingletonScript)))
                {
                    servicesToInstanciate.Add(type);
                }

                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }

        private static readonly List<Type> servicesToInstanciate = new List<Type>();

        public static void InstanciateStartupScripts(this ServiceProvider provider)
        {
            foreach (var type in servicesToInstanciate)
            {
                _ = provider.GetService(type);
            }
        }
    }
}
