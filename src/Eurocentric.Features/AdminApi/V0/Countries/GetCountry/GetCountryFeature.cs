using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Countries.GetCountry;

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
            Country? country = await dbContext.V0Countries.AsNoTracking()
                .Where(country => country.Id == query.CountryId)
                .Select(country => country.ToCountryDto())
                .SingleOrDefaultAsync(cancellationToken);

            return country is null ? CountryNotFound(query.CountryId) : new GetCountryResponse(country);
        }

        private static Error CountryNotFound(Guid countryId) => Error.NotFound("Country not found",
            "No country exists with the provided country ID.",
            new Dictionary<string, object> { { nameof(countryId), countryId } });
    }
}
