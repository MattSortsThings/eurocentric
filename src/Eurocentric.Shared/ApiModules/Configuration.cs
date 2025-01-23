using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiModules;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Configuration
{
    /// <summary>
    ///     Configures the web application to use the versioned API endpoints that were configured before the application was
    ///     built.
    /// </summary>
    /// <param name="app">The web application to be configured.</param>
    public static void UseVersionedApiEndpoints(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        foreach (IApiModule module in scope.ServiceProvider.GetRequiredService<IEnumerable<IApiModule>>())
        {
            module.MapVersionedApiEndpoints(app);
        }
    }
}
