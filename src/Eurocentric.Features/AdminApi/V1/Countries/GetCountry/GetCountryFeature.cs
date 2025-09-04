using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using Country = Eurocentric.Domain.Aggregates.Countries.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

internal static class GetCountryFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(countryId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid CountryId) : IQuery<GetCountryResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Country dummyCountry = Country.CreateDummyCountry(query.CountryId);

            return new GetCountryResponse(dummyCountry.ToCountryDto());
        }
    }
}
