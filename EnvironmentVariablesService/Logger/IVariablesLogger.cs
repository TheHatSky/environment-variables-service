namespace EnvironmentVariablesService
{
    public interface IVariablesLogger
    {
        void Log<T>(EnvironmentVariable<T> variable);
        void LogError<T>(EnvironmentVariable<T> variable);
    }
}
