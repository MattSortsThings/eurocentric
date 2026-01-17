using CSharpFunctionalExtensions;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Placeholders;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Public.V0.Placeholders;

[Mapper]
internal static partial class PingPublicApi
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<QueryResult, DomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.MapToResponseBody())
            : TypedResults.InternalServerError("Request failed");
    }

    private static partial PingPublicApiResponseBody MapToResponseBody(this QueryResult result);

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("ping", ExecuteAsync)
                .WithSummary("Ping Public API")
                .WithName("PublicApi.V0.PingPublicApi")
                .WithDisplayName("PublicApi:V0:PingPublicApi")
                .Produces<PingPublicApiResponseBody>();
        }
    }

    internal readonly record struct QueryResult(string ApiName, List<string> Items);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "Messaging")]
    internal sealed class QueryHandler : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            const string apiName = "Public API";
            List<string> messages = BlobbyGenerator.Generate(3);

            return new QueryResult(apiName, messages);
        }
    }
}
