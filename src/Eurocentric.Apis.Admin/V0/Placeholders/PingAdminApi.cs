using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EFCore;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Placeholders;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Placeholders;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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
        Result<QueryResult, DomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.MapToResponseBody())
            : TypedResults.InternalServerError("Request failed");
    }

    private static partial PingAdminApiResponseBody MapToResponseBody(this QueryResult result);

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

    internal readonly record struct QueryResult(string ApiName, List<string> Items);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "Messaging")]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            int countryCount = await dbContext.Set<Country>().AsNoTracking().CountAsync(cancellationToken);

            if (countryCount != 0)
            {
                throw new InvalidOperationException("Illegal country count!");
            }

            const string apiName = "Admin API";
            List<string> messages = BlobbyGenerator.Generate(4);

            return new QueryResult(apiName, messages);
        }
    }
}
