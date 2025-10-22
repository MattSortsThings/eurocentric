using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Functional;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

internal static class GetCountry
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(countryId), MapToOk, ct);

    private static Ok<GetCountryResponse> MapToOk(Guid countryId)
    {
        Country countryDto = Country.CreateExample() with { Id = countryId };

        return TypedResults.Ok(new GetCountryResponse(countryDto));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("countries/{countryId:guid}", ExecuteAsync)
                .WithName(V1EndpointNames.Countries.GetCountry)
                .AddedInVersion1Point0()
                .WithSummary("Get a countries")
                .WithDescription("Retrieves the requested country in the system.")
                .WithTags(V1Tags.Countries)
                .Produces<GetCountryResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record Query(Guid CountryId) : IQuery<Guid>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, Guid>
    {
        public async Task<Result<Guid, IDomainError>> OnHandle(Query query, CancellationToken ct)
        {
            await Task.CompletedTask;

            return query.CountryId;
        }
    }
}
