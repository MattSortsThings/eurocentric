using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ErrorHandling;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the error handling services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Instance = ctx.HttpContext.Request.GetInstance();
            };
        });

        return services;
    }

    private static string GetInstance(this HttpRequest request) => $"{request.Method} {request.Path}{request.QueryString}";
}
