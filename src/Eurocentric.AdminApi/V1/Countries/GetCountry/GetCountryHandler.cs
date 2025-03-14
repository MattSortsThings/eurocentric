using ErrorOr;
using Eurocentric.AdminApi.V1.Models;
using Eurocentric.DataAccess.EfCore;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Shared.AppPipeline;
using Microsoft.EntityFrameworkCore;
using Country = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.AdminApi.V1.Countries.GetCountry;

internal sealed class GetCountryHandler(AppDbContext dbContext) : QueryHandler<GetCountryQuery, GetCountryResult>
{
    public override async Task<ErrorOr<GetCountryResult>> Handle(GetCountryQuery query, CancellationToken cancellationToken)
    {
        CountryId countryId = CountryId.FromValue(query.CountryId);

        Country? country = await dbContext.Countries.AsNoTracking()
            .Where(country => country.Id.Equals(countryId))
            .SingleOrDefaultAsync(cancellationToken);

        return country is not null
            ? new GetCountryResult(country.ToModelCountry())
            : DomainErrors.Countries.CountryNotFound(countryId);
    }
}
