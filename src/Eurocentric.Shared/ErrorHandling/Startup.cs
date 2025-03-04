using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ErrorHandling;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the error handling services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(ConfigureProblemDetailsOptions)
            .AddExceptionHandler<BadHttpRequestExceptionHandler>()
            .AddExceptionHandler<FallbackExceptionHandler>();

        return services;
    }

    private static void ConfigureProblemDetailsOptions(ProblemDetailsOptions options) =>
        options.CustomizeProblemDetails = context => context.ProblemDetails.Instance = context.HttpContext.Request.GetInstance();
}
