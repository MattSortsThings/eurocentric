using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.QueryableCountries.GetQueryableCountries;

internal static class GetQueryableCountriesEndpoint
{
    internal static void MapGetQueryableCountriesV0Point1(this IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapGet("v0.1/queryable-countries",
                static async (IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<GetQueryableCountriesResponse> result = await bus.Send(new GetQueryableCountriesQuery(),
                        cancellationToken: cancellationToken);

                    return TypedResults.Ok(result.Value);
                }).WithSummary("Get queryable countries")
            .WithTags("Queryable Countries")
            .WithName("GetQueryableCountriesV0Point1");
}
