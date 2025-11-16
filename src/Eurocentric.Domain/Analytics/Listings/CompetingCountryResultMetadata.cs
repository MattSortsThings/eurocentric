namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Metadata describing an executed competing country result listings query.
/// </summary>
public sealed record CompetingCountryResultMetadata
{
    /// <summary>
    ///     The required competing country code filter value.
    /// </summary>
    public string CompetingCountryCode { get; init; } = string.Empty;
}
