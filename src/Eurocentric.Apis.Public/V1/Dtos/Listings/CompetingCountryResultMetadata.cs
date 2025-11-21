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
}
