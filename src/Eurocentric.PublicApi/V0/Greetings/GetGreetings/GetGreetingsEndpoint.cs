using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

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

    public static void MapPublicApiPlaceholderEndpoint(this WebApplication app) => app
        .MapGet("public/api/v0.1/greetings", ExecuteAsync)
        .WithSummary("Get greetings")
        .WithTags("Greetings");
}
