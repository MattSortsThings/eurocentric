using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     Metadata describing an executed competing country result listings query.
/// </summary>
public sealed record CompetingCountryResultMetadata : IDtoSchemaExampleProvider<CompetingCountryResultMetadata>
{
    /// <summary>
    ///     The required competing country code filter value.
    /// </summary>
    public string CompetingCountryCode { get; init; } = string.Empty;

    public static CompetingCountryResultMetadata CreateExample() => new() { CompetingCountryCode = "AT" };

    public bool Equals(CompetingCountryResultMetadata? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return CompetingCountryCode == other.CompetingCountryCode;
    }

    public override int GetHashCode() => CompetingCountryCode.GetHashCode();
}
