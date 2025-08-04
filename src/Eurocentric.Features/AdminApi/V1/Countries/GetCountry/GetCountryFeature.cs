using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

internal static class GetCountryFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "countryId")] Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(countryId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid CountryId) : IQuery<GetCountryResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            CountryDto dummyCountry = CreateDummyCountry(query.CountryId);

            return new GetCountryResponse(dummyCountry);
        }

        private static CountryDto CreateDummyCountry(Guid countryId)
        {
            CountryAggregate aggregate = new(CountryId.FromValue(countryId), CountryCode.FromValue("AT").Value,
                CountryName.FromValue("Austria").Value);

            return aggregate.ToCountryDto();
        }
    }
}
