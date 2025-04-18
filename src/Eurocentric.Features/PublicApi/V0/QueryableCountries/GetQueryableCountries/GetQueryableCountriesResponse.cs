using Eurocentric.Features.PublicApi.V0.QueryableCountries.Models;

namespace Eurocentric.Features.PublicApi.V0.QueryableCountries.GetQueryableCountries;

public sealed record GetQueryableCountriesResponse(int TotalItems, QueryableCountry[] QueryableCountries);
