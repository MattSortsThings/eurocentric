using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountries;

internal static class GetCountriesFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetCountriesResponse>;

    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetCountriesResponse>
    {
        public async Task<ErrorOr<GetCountriesResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext.Countries.AsNoTracking()
                .AsSplitQuery()
                .OrderBy(country => country.CountryCode)
                .Select(country => country.ToCountryDto())
                .ToArrayAsync(cancellationToken);

            return new GetCountriesResponse(countries);
        }
    }
}
