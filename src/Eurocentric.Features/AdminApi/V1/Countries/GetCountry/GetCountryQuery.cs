using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

public sealed record GetCountryQuery(Guid CountryId) : Request<GetCountryResponse>;
