using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;

/// <summary>
///     A dictionary containing country IDs, accessible by country codes.
/// </summary>
public sealed class CountryIdLookup
{
    private readonly Dictionary<string, Guid> _lookup;

    public CountryIdLookup(int capacity = 0)
    {
        _lookup = new Dictionary<string, Guid>(capacity);
    }

    public void Add(Country country) => _lookup.Add(country.CountryCode, country.Id);

    public Guid GetSingle(string countryCode) => _lookup[countryCode];
}
