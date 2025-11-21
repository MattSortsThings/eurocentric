using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

public sealed record CreateCountryRequest : IDtoSchemaExampleProvider<CreateCountryRequest>
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() => new() { CountryCode = "AT", CountryName = "Austria" };

    public bool Equals(CreateCountryRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return CountryCode == other.CountryCode && CountryName == other.CountryName;
    }

    public override int GetHashCode() => HashCode.Combine(CountryCode, CountryName);
}
