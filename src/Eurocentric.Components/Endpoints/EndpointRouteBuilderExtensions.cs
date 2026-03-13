using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Endpoints;

/// <summary>
///     Extension methods for the <see cref="IEndpointRouteBuilder" /> interface.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    ///     Maps one or more endpoints to the endpoint route builder, as specified in the <typeparamref name="TMapper" />
    ///     endpoint mapper type.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <typeparam name="TMapper">The endpoint mapper type.</typeparam>
    /// <returns>The original <see cref="IEndpointRouteBuilder" /> instance.</returns>
    public static IEndpointRouteBuilder Map<TMapper>(this IEndpointRouteBuilder builder)
        where TMapper : IEndpointMapper
    {
        TMapper.MapTo(builder);

        return builder;
    }
}
