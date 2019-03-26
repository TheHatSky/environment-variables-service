using Microsoft.Extensions.DependencyInjection;
using System;

namespace EnvironmentVariablesService
{
    public class TestScopeVariables : VariablesScope
    {
        public const string STRING_VALUE_NAME = "VALUE";

        public const string INT_VALUE_NAME = "INT_VALUE";
        public const int DefaultIntValue = 80;

        public const string NULLABLE_INT_VALUE_NAME = "NULLABLE_INT_VALUE";
        public static readonly int? DefaultNullableIntValue = 2812;

        protected override string Scope => null;

        public EnvironmentVariable<string> STRING_VALUE { get; }
        public EnvironmentVariable<int> INT_VALUE { get; }
        public EnvironmentVariable<int?> NULLABLE_INT_VALUE { get; }

        public TestScopeVariables()
        {
            STRING_VALUE = Add<string>(STRING_VALUE_NAME);
            INT_VALUE = Add<int>(INT_VALUE_NAME, false, DefaultIntValue);
            NULLABLE_INT_VALUE = Add<int?>(NULLABLE_INT_VALUE_NAME, false, DefaultNullableIntValue);
        }
    }
}
