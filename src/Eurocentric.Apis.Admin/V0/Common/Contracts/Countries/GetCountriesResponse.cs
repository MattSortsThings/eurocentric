using Eurocentric.Apis.Admin.V0.Common.Config;

namespace Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;

/// <summary>
///     Successful response body for the <see cref="EndpointIds.Countries.GetCountries" /> endpoint.
/// </summary>
/// <param name="Countries">The retrieved countries.</param>
public sealed record GetCountriesResponse(Country[] Countries);
