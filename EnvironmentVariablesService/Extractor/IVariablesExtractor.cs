using Microsoft.Extensions.DependencyInjection;
using System;

namespace EnvironmentVariablesService
{
    public interface IVariablesExtractor
    {
        (T result, string errorMessage) Get<T>(string name);
    }
}
