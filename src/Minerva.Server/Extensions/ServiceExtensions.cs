using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minerva.Server.Contracts.ScriptStrategy;
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
        }

        public static void AddAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where T : class
        {
            var typesOfInterface = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.DefinedTypes)
                .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(T)));

            foreach (var type in typesOfInterface)
            {
                _logger.LogTrace($"Dependency Injection: Registering {type.Name} (implements interface {typeof(T).Name})");

                if (type.ImplementedInterfaces.Contains(typeof(IStartupSingletonScript)))
                {
                    servicesToInstanciate.Add(type);
                    _logger.LogTrace("... and configured for instanciation on startup");
                }

                // add as resolvable by implementation type
                services.Add(new ServiceDescriptor(type, type, lifetime));

                if (typeof(T) != type)
                {
                    // add as resolvable by service type
                    services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
                }
            }

            var typesOfClasses = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T)));

            foreach (var type in typesOfClasses)
            {
                _logger.LogTrace($"Dependency Injection: Registering {type.Name} (inherits class {typeof(T).Name})");

                if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IStartupSingletonScript)))
                {
                    servicesToInstanciate.Add(type);

                    _logger.LogTrace("... and configured for instanciation on startup");
                }

                // add as resolvable by implementation type
                services.Add(new ServiceDescriptor(type, type, lifetime));

                if (typeof(T) != type)
                {
                    // add as resolvable by service type
                    services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
                }
            }
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
