using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EnvironmentVariablesService
{
    public class VariablesExtractor : IVariablesExtractor
    {
        private IReadOnlyDictionary<Type, Func<string, (object, string)>> valueProviders = new Dictionary<Type, Func<string, (object, string)>>
        {
            [typeof(int)] = GetInt32,
            [typeof(int?)] = GetNullableInt32,
            [typeof(long)] = GetInt64,
            [typeof(bool)] = GetBoolean,
            [typeof(DateTime)] = GetDateTime,
            [typeof(string)] = GetString
        };

        public (T result, string errorMessage) Get<T>(string variableName)
        {
            if (variableName == null)
                throw new ArgumentNullException(nameof(variableName));

            var type = typeof(T);
            var allowedTypes = valueProviders.Keys;

            if (allowedTypes.Contains(type))
            {
                var (resultObject, errorMessage) = valueProviders[type](variableName);

                return errorMessage == null
                    ? ((T)resultObject, null)
                    : (default(T), errorMessage);
            }

            return (default(T), $"Type '{type.Name}' is not supported for environment variable deserialization.");
        }

        private static (object, string) GetNullableInt32(string variableName)
        {
            var (serializedValueObject, errorMessage) = GetString(variableName);
            var serializedValue = serializedValueObject as string;

            if (errorMessage != null)
                return (default(Int32?), errorMessage);

            if (serializedValue == null)
                return (null, null);

            if (Int32.TryParse(serializedValue as string, out int result))
                return (result, null);

            return (default(Int32), $"Value '{serializedValue}' can't be parsed to Nullable<Int32>.");
        }

        private static (object, string) GetInt32(string variableName)
        {
            var (result, errorMessage) = GetNullableInt32(variableName);

            if (result == null)
                return (default(Int32), $"Value '{result}' can't be parsed to Int32.");

            return ((int)result, null);
        }

        private static (object, string) GetInt64(string variableName)
        {
            var (serializedValue, errorMessage) = GetString(variableName);

            if (errorMessage != null)
                return (default(Int64), errorMessage);

            if (Int64.TryParse(serializedValue as string, out long result))
                return (result, null);

            return (default(Int64), $"Value '{serializedValue}' can't be parsed to Int64.");
        }

        private static (object, string) GetBoolean(string variableName)
        {
            var (serializedValue, errorMessage) = GetString(variableName);

            if (errorMessage != null)
                return (default(Boolean), errorMessage);

            if (Boolean.TryParse(serializedValue as string, out bool result))
                return (result, null);

            return (default(Boolean), $"Value '{serializedValue}' can't be parsed to Boolean.");
        }

        private static (object, string) GetDateTime(string variableName)
        {
            var (serializedValue, errorMessage) = GetString(variableName);

            if (errorMessage != null)
                return (default(DateTime), errorMessage);

            if (DateTime.TryParseExact(
                serializedValue as string,
                @"yyyy-MM-dd\THH:mm:ss\Z",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out DateTime unspecifiedResult))
            {
                var result = DateTime.SpecifyKind(
                    unspecifiedResult,
                    DateTimeKind.Utc
                );
                return (result, null);
            }

            return (default(DateTime), $"Value '{serializedValue}' can't be parsed to DateTime using format 'yyyy-MM-dd\\THH:mm:ss\\Z'.");
        }

        private static (object, string) GetString(string variableName)
        {
            var variable = Environment
                .GetEnvironmentVariable(variableName);

            variable = variable == null ? null : variable.Trim();

            var result = variable == string.Empty
                ? null
                : variable;

            return (result, null);
        }
    }
}
