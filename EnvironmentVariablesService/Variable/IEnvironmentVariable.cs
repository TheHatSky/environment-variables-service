namespace EnvironmentVariablesService
{
    internal interface IEnvironmentVariable
    {
        void Read(IVariablesExtractor extractor);
        void Log(IVariablesLogger logger);

        bool HasError { get; }
    }
}