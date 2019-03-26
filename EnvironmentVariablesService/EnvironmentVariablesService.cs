using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EnvironmentVariablesService
{
    public class EnvironmentVariablesService
    {
        private IVariablesExtractor Extractor { get; set; }
        private IVariablesLogger Logger { get; set; }
        private IServiceCollection Services { get; }
        private IList<VariablesScope> Scopes { get; } = new List<VariablesScope>();

        public EnvironmentVariablesService(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
            Extractor = new VariablesExtractor();
            Logger = new VariablesLogger();
        }

        public EnvironmentVariablesService AddScope<TScope>()
            where TScope : VariablesScope, new()
        {
            var scope = new TScope();

            Scopes.Add(scope);

            Services
                .AddSingleton<TScope>(p => scope);

            return this;
        }

        public EnvironmentVariablesService WithLogger<TLogger>()
            where TLogger : IVariablesLogger, new()
        {
            return WithLogger(new TLogger());
        }

        public EnvironmentVariablesService WithLogger<TLogger>(TLogger logger)
            where TLogger : IVariablesLogger
        {
            Logger = logger;
            return this;
        }

        public EnvironmentVariablesService WithExtractor<TExtractor>()
            where TExtractor : IVariablesExtractor, new()
        {
            return WithExtractor(new TExtractor());
        }

        public EnvironmentVariablesService WithExtractor<TExtractor>(TExtractor extractor)
            where TExtractor : IVariablesExtractor
        {
            Extractor = extractor;
            return this;
        }

        public IServiceCollection Read()
        {
            foreach (var scope in Scopes)
            {
                scope.Read(Extractor);
            }

            foreach (var scope in Scopes)
            {
                scope.Log(Logger);
            }

            return Services;
        }
    }
}
