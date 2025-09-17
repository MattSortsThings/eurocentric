using ErrorOr;

namespace Eurocentric.Domain.V0Entities;

public sealed record Country
{
    public Guid Id { get; init; }

    public string CountryCode { get; init; } = null!;

    public string CountryName { get; init; } = null!;

    public List<ContestRole> ContestRoles { get; init; } = [];

    public static ErrorOr<Country> CreateReal(string countryCode, string countryName)
    {
        if (LegalCountryCodeValue(countryCode))
        {
            return new Country { Id = Guid.NewGuid(), CountryCode = countryCode, CountryName = countryName };
        }

        return IllegalCountryCodeValue(countryCode);
    }

    public static ErrorOr<Country> CreatePseudo(string countryCode, string countryName)
    {
        if (LegalCountryCodeValue(countryCode))
        {
            return new Country { Id = Guid.NewGuid(), CountryCode = countryCode, CountryName = countryName };
        }

        return IllegalCountryCodeValue(countryCode);
    }

    private static bool LegalCountryCodeValue(string countryCode) =>
        countryCode.Length == 2 && countryCode.All(char.IsAsciiLetterUpper);

    private static Error IllegalCountryCodeValue(string countryCode) => Error.Failure("Illegal country code value",
        "Country code value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { { nameof(countryCode), countryCode } });
}
