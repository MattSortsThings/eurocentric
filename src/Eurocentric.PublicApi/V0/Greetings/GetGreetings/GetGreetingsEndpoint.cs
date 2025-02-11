using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

public static class GetGreetingsEndpoint
{
    public static async Task<Ok<GetGreetingsResponse>> ExecuteAsync([AsParameters] GetGreetingsRequest request,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        GetGreetingsResponse result = await sender.Send(request.ToQuery(), cancellationToken);

        return TypedResults.Ok(result);
    }

    public static void MapGetGreetings(this IEndpointRouteBuilder app) => app
        .MapGet("greetings", ExecuteAsync)
        .WithSummary("Get greetings")
        .WithTags("Greetings");
}
