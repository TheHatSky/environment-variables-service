using Microsoft.Extensions.DependencyInjection;
using System;

namespace EnvironmentVariablesService
{
    public static class ServiceExtensions
    {
        public static EnvironmentVariablesService AddEnvVariables(this IServiceCollection serviceCollection)
        {
            return new EnvironmentVariablesService(serviceCollection);
        }
    }
}
