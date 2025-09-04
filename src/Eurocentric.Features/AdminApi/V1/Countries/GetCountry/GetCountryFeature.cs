using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Country = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

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
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            CountryId countryId = CountryId.FromValue(query.CountryId);

            Country? country = await dbContext.Countries.AsNoTracking()
                .Where(country => country.Id == countryId)
                .Select(country => country.ToCountryDto())
                .FirstOrDefaultAsync(cancellationToken);

            return country is null ? CountryErrors.CountryNotFound(countryId) : new GetCountryResponse(country);
        }
    }
}
