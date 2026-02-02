using Eurocentric.Apis.Public.V0.Common.Constants;
using Eurocentric.Components.Endpoints;
using Eurocentric.Domain.Placeholders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0.Placeholders;

internal static class GetBlobbies
{
    private static Task<IResult> ExecuteAsync(
        [AsParameters] GetBlobbiesQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        GetBlobbiesResponseBody responseBody = new(BlobbyGenerator.Generate(queryParams.Count));

        IResult result = TypedResults.Ok(responseBody);

        return Task.FromResult(result);
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("blobbies", ExecuteAsync)
                .WithName(EndpointNames.GetBlobbies)
                .WithDisplayName(EndpointDisplayNames.GetBlobbies)
                .WithTags(EndpointTags.Placeholders)
                .Produces<GetBlobbiesResponseBody>();
        }
    }
}
