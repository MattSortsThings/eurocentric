using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.ErrorHandling;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the error handling services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(static options => options.CustomizeProblemDetails = CustomizeInstance);

        return services;
    }

    private static void CustomizeInstance(ProblemDetailsContext context)
    {
        HttpRequest request = context.HttpContext.Request;

        context.ProblemDetails.Instance = $"{request.Method} {request.Path}{request.QueryString}";
    }
}
