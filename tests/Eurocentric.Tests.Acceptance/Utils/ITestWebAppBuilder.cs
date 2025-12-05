using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Tests.Acceptance.Utils;

public interface ITestWebAppBuilder
{
    ITestWebAppBuilder WithExtraConfiguration(Action<IServiceCollection> configuration);

    Task<TestWebApp> InitializeAsync();
}
