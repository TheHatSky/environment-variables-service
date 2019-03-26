using System;

namespace EnvironmentVariablesService
{
    public class VariablesLogger : IVariablesLogger
    {
        public void Log<T>(EnvironmentVariable<T> variable)
        {
            if (variable == null)
                throw new ArgumentNullException(nameof(variable));

            Console.WriteLine(
                $"[{LocalDateTime}] [INF] {variable.FullName} = {variable}"
            );
        }

        public void LogError<T>(EnvironmentVariable<T> variable)
        {
            if (variable == null)
                throw new ArgumentNullException(nameof(variable));

            Console.WriteLine(
                $"[{LocalDateTime}] [ERR] {variable.FullName}: {variable.ErrorMessage}"
            );
        }

        private string LocalDateTime => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
    }
}
