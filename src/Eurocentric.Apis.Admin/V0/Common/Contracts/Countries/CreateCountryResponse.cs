using Eurocentric.Apis.Admin.V0.Common.Config;

namespace Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;

/// <summary>
///     Successful response body for the <see cref="EndpointIds.Countries.CreateCountryV0Point1" /> endpoint.
/// </summary>
/// <param name="Country">The created country.</param>
public sealed record CreateCountryResponse(Country Country);
