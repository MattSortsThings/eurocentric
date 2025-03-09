using System.ComponentModel;
using Eurocentric.Domain.Countries;
using ModelCountry = Eurocentric.AdminApi.V1.Models.Country;
using DomainCountry = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.AdminApi.V1.Models;

internal static class Mapping
{
    internal static ModelCountry ToModelCountry(this DomainCountry country) =>
        new(country.Id.Value,
            country.CountryCode.Value,
            country.CountryName.Value,
            Enum.Parse<CountryType>(country.CountryType.ToString()),
            country.ContestIds.Select(id => id.Value).ToArray());

    internal static ICountryBuilder ToBuilder(this CountryType countryType) => countryType switch
    {
        CountryType.Real => DomainCountry.CreateReal(),
        CountryType.Pseudo => DomainCountry.CreatePseudo(),
        _ => throw new InvalidEnumArgumentException("countryType", (int)countryType, typeof(CountryType))
    };
}
