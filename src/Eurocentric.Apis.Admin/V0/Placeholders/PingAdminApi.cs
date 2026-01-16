using CSharpFunctionalExtensions;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Placeholders;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Placeholders;

[Mapper]
internal static partial class PingAdminApi
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<Result> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.MapToResponseBody())
            : TypedResults.InternalServerError("Request failed");
    }

    private static partial PingAdminApiResponseBody MapToResponseBody(this Result result);

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("ping", ExecuteAsync)
                .WithSummary("Ping Admin API")
                .WithName("AdminApi.V0.PingAdminApi")
                .WithDisplayName("AdminApi:V0:PingAdminApi")
                .Produces<PingAdminApiResponseBody>();
        }
    }

    internal readonly record struct Result(string ApiName, List<string> Items);

    internal sealed record Query : IRequest<Result<Result>>;

    [UsedImplicitly(Reason = "Messaging")]
    internal sealed class QueryHandler : IRequestHandler<Query, Result<Result>>
    {
        public async Task<Result<Result>> OnHandle(Query request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            const string apiName = "Admin API";
            List<string> items = BlobbyGenerator.Generate(4);

            return new Result(apiName, items);
        }
    }
}
