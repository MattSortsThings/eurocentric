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

namespace Eurocentric.Features.AdminApi.V0.Countries.GetCountries;

internal static class GetCountriesFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetCountriesResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetCountriesResponse>
    {
        public async Task<ErrorOr<GetCountriesResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext.V0Countries.AsNoTracking()
                .OrderBy(country => country.CountryCode)
                .Select(country => country.ToCountryDto())
                .ToArrayAsync(cancellationToken);

            return new GetCountriesResponse(countries);
        }
    }
}
