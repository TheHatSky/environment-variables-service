using System;
using System.Collections.Generic;
using System.Linq;

namespace EnvironmentVariablesService
{
    public abstract class VariablesScope
    {
        private List<IEnvironmentVariable> Variables { get; } = new List<IEnvironmentVariable>();
        protected abstract string Scope { get; }

        protected EnvironmentVariable<T> Add<T>(
            string name,
            bool required = true,
            T defaultValue = default(T))
        {
            var variable = new EnvironmentVariable<T>(
                Scope,
                name,
                defaultValue,
                required
            );

            Variables.Add(variable);

            return variable;
        }

        internal void Read(IVariablesExtractor extractor)
        {
            foreach (var variable in Variables)
            {
                variable.Read(extractor);
            }
        }

        internal void Log(IVariablesLogger logger)
        {
            foreach (var variable in Variables)
            {
                variable.Log(logger);
            }

            var variablesWithErrors = Variables
                .Where(v => v.HasError)
                .ToArray();

            if (variablesWithErrors.Any())
                throw new ApplicationException(
                    $"Error while reading environment variables."
                );
        }
    }
}
