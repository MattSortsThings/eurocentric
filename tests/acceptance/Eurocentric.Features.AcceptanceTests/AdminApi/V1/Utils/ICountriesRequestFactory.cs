using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     Creates REST requests for "Countries" tagged endpoints.
/// </summary>
public interface ICountriesRequestFactory
{
    /// <summary>
    ///     Creates a REST request for the "CreateCountry" endpoint.
    /// </summary>
    /// <param name="requestBody">The request body.</param>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest CreateCountry(CreateCountryRequest requestBody);

    /// <summary>
    ///     Creates a REST request for the "GetCountries" endpoint.
    /// </summary>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest GetCountries();

    /// <summary>
    ///     Creates a REST request for the "GetCountry" endpoint.
    /// </summary>
    /// <param name="countryId">The country ID.</param>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest GetCountry(Guid countryId);
}
