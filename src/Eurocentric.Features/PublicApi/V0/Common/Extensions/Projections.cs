using System.Linq.Expressions;
using Eurocentric.Domain.PlaceholderEntities;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;

namespace Eurocentric.Features.PublicApi.V0.Common.Extensions;

internal static class Projections
{
    internal static Expression<Func<QueryableCountry, Country>> ProjectToCountryDto =>
        country => new Country(country.CountryCode, country.CountryName);
}
