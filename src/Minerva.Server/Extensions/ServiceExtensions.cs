using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minerva.Server.Core.ScriptStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Minerva.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ILogger _logger;
        private static readonly Dictionary<Type, object> _singletonInstances;

        static ServiceCollectionExtensions()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConfiguration(config.GetSection("Logging"))
                    .AddDebug()
                    .AddConsole();
            });

            _logger = loggerFactory.CreateLogger(typeof(ServiceCollectionExtensions));

            _singletonInstances = new Dictionary<Type, object>();
        }

        public static void AddAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where T : class
        {
            #region T is interface

            var typesOfInterface = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.DefinedTypes)
                .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(T)));

            foreach (var type in typesOfInterface)
            {
                _logger.LogTrace($"Registering {type.Name} (implements interface {typeof(T).Name}) with lifetime {Enum.GetName(lifetime)}");

                if (services.Any(e => e.ServiceType == type))
                {
                    _logger.LogDebug($"Skipping registration of {type.Name} -> already registered!");
                    continue;
                }

                if (type.ImplementedInterfaces.Contains(typeof(IStartupSingletonScript)))
                {
                    servicesToInstanciate.Add(type);
                    lifetime = ServiceLifetime.Singleton;

                    _logger.LogTrace($"Configured {type.Name} for instanciation on startup");
                }

                // add as resolvable by implementation type
                services.Add(new ServiceDescriptor(type, type, lifetime));

                if (typeof(T) != type)
                {
                    // add as resolvable by service type (forwarding)
                    services.Add(new ServiceDescriptor(typeof(T), x => x.GetRequiredService(type), lifetime));
                }
            }

            #endregion

            #region T is class

            var typesOfClasses = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T)));

            foreach (var type in typesOfClasses)
            {
                _logger.LogTrace($"Registering {type.Name} (inherits class {typeof(T).Name}) with lifetime {Enum.GetName(lifetime)}");

                if (services.Any(e => e.ServiceType == type))
                {
                    _logger.LogDebug($"Skipping registration of {type.Name} -> already registered!");
                    continue;
                }
                if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IStartupSingletonScript)))
                {
                    servicesToInstanciate.Add(type);
                    lifetime = ServiceLifetime.Singleton;

                    _logger.LogTrace($"Configured {type.Name} for instanciation on startup");
                }

                // add as resolvable by implementation type
                services.Add(new ServiceDescriptor(type, type, lifetime));

                if (typeof(T) != type)
                {
                    // add as resolvable by service type (forwarding)
                    services.Add(new ServiceDescriptor(typeof(T), x => x.GetRequiredService(type), lifetime));
                }
            }

            #endregion
        }

        private static object GetSingletonInstance(IServiceProvider serviceProvider, Type type)
        {
            if (_singletonInstances.ContainsKey(type))
            {
                return _singletonInstances[type];
            }

            var instance = serviceProvider.GetRequiredService(type);
            _singletonInstances.Add(type, instance);

            return instance;
        }

        private static readonly List<Type> servicesToInstanciate = new List<Type>();

        public static void InstanciateStartupScripts(this ServiceProvider provider)
        {
            _logger.LogTrace("Dependency Injection: Instanciating registered scripts");

            foreach (var type in servicesToInstanciate)
            {
                _ = provider.GetService(type);

                _logger.LogTrace($"Instanciated {type.Name}");
            }
        }
    }
}
