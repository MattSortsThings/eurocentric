using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Endpoints;

/// <summary>
///     Extension methods for the <see cref="IEndpointRouteBuilder" /> interface.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    ///     Maps one or more endpoints to the endpoint route builder as specified by the endpoint mapper type.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <typeparam name="TMapper">The endpoint mapper type.</typeparam>
    /// <returns>The original <see cref="IEndpointRouteBuilder" /> instance.</returns>
    public static IEndpointRouteBuilder Map<TMapper>(this IEndpointRouteBuilder builder)
        where TMapper : IEndpointMapper, new()
    {
        TMapper mapper = new();

        mapper.Map(builder);

        return builder;
    }
}
