## Motivation

I've found lack of handful environment managment in asp.net core.  
Beeing inspired by [The Twelve-Factor App](https://12factor.net/) I've wrote this `IServiceCollection` extension.

## How to use

Create class derived from `VariableScope` and register your variables.

```csharp
using EnvironmentVariablesService;

namespace MyApp
{
    public class GraphQLSettings : VariablesScope
    {
        // Optional. Can be null.
        protected override string Scope => "GRAPHQL";

        private const string PlaygroundEnabledName = "PLAYGROUND_ENABLED";

        public EnvironmentVariable<bool> PlaygroundEnabled { get; set; }

        public GraphQLSettings()
        {
            // Will read from env GRAPHQL_PLAYGROUND_ENABLED
            PlaygroundEnabled = Add<bool>(
                PlaygroundEnabledName,
                required: false,
                defaultValue: true);
        }
    }
}
```

Call `AddEnvVariables` extension:

```csharp
public IServiceProvider ConfigureServices(IServiceCollection services) =>
    services
        .AddEnvVariables()
            .AddScope<GraphQLSettings>()
            // Add as many scopes as you want
            .AddScope<DbSettings>()
            .Read()
        ...
        .BuildServiceProvider();
```

After `Read` invocation you can use all your scopes as a regisetred service.  
Any scope can be aquired through dependency injection, from `IServiceCollection` and `IServiceProvider`.

```csharp
public void Configure(IApplicationBuilder application)
{
    bool playgroundEnabled = application
        .ApplicationServices
        .GetService<GraphQLSettings>()
        .PlaygroundEnabled; // Implicit cast to bool

    application
        .UseGraphQL<MainSchema>()
        ...
}
```
