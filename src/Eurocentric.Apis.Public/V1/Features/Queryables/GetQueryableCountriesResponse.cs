using Eurocentric.Apis.Public.V1.Dtos.Queryables;

namespace Eurocentric.Apis.Public.V1.Features.Queryables;

public sealed record GetQueryableCountriesResponse(QueryableCountry[] QueryableCountries);
