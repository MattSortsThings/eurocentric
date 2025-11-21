using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public sealed record CreateCountryRequest : IDtoSchemaExampleProvider<CreateCountryRequest>
{
    public required CountryType CountryType { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() =>
        new()
        {
            CountryType = CountryType.Real,
            CountryCode = "AT",
            CountryName = "Austria",
        };

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

        return CountryType == other.CountryType && CountryCode == other.CountryCode && CountryName == other.CountryName;
    }

    public override int GetHashCode() => HashCode.Combine((int)CountryType, CountryCode, CountryName);
}
