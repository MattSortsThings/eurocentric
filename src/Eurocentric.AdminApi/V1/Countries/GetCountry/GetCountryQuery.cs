using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V1.Countries.GetCountry;

public sealed record GetCountryQuery(Guid CountryId) : Query<GetCountryResult>;
