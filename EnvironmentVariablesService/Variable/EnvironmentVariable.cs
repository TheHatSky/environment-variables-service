using System;

namespace EnvironmentVariablesService
{
    public class EnvironmentVariable<T> : IEnvironmentVariable
    {
        public T Value { get; private set; }

        public string FullName =>
            Scope != null
                ? $"{Scope}_{Name}"
                : Name;

        private string Scope { get; }
        private T DefaultValue { get; }
        private bool Required { get; }

        private string Name { get; }

        public bool HasError => ErrorMessage != null;

        internal string ErrorMessage { get; private set; }
        internal bool Initialized { get; private set; }


        internal EnvironmentVariable(
            string scope,
            string name,
            T defaultValue,
            bool required
        )
        {
            Scope = scope;
            Name = name;
            DefaultValue = defaultValue;
            Required = required;
        }

        public static implicit operator T(EnvironmentVariable<T> variable)
        {
            return variable.Value;
        }

        public override string ToString()
        {
            var type = typeof(T);
            var typeName = type.IsGenericType
                ? $"{type.Name}<{type.GenericTypeArguments[0].Name}>"
                : type.Name;

            if (Value == null)
                return $"({typeName}) `null`";

            return $"({typeName}) `{Value.ToString()}`";
        }

        public void Read(IVariablesExtractor extractor)
        {
            var (result, errorMessage) = extractor.Get<T>(FullName);

            ErrorMessage = errorMessage;
            Value = result == null ? DefaultValue : result;
            Initialized = true;
        }

        public void Log(IVariablesLogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (!Initialized)
                throw new InvalidOperationException(
                    $"Variable {FullName} wasn't read before logging."
                );

            if (HasError)
                logger.LogError(this);
            else
                logger.Log(this);
        }
    }
}
