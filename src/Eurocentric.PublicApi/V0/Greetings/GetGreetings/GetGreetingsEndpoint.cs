using ErrorOr;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

internal static class GetGreetingsEndpoint
{
    internal static void MapGetGreetings(this IEndpointRouteBuilder api) =>
        api.MapGet("public/api/v0.1/greetings",
                async ([AsParameters] GetGreetingsQuery query,
                    ISender sender,
                    CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<GetGreetingsResult> result = await sender.Send(query, cancellationToken);

                    return result.ToHttpResult(TypedResults.Ok);
                })
            .WithSummary("Get greetings")
            .WithTags("Greetings")
            .Produces<GetGreetingsResult>();
}
