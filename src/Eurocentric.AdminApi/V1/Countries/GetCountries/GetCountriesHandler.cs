using ErrorOr;
using Eurocentric.AdminApi.V1.Models;
using Eurocentric.DataAccess.EfCore;
using Eurocentric.Shared.AppPipeline;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.AdminApi.V1.Countries.GetCountries;

internal sealed class GetCountriesHandler(AppDbContext dbContext) : QueryHandler<GetCountriesQuery, GetCountriesResult>
{
    public override async Task<ErrorOr<GetCountriesResult>> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
    {
        Country[] countries = await dbContext.Countries.AsNoTracking()
            .OrderBy(c => c.CountryCode)
            .Select(country => country.ToModelCountry())
            .ToArrayAsync(cancellationToken);

        return new GetCountriesResult(countries);
    }
}
