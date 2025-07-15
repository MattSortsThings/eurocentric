using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     A dictionary in which each value is a country and the corresponding key is its country code.
/// </summary>
public sealed class CountryLookup : Dictionary<string, Country>
{
    public Guid GetId(string countryCode) => this[countryCode].Id;

    public IEnumerable<Country> GetAllCountries() => Values;
}
