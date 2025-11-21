using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     Metadata describing an executed competing country points listings query.
/// </summary>
public sealed record CompetingCountryPointsMetadata : IDtoSchemaExampleProvider<CompetingCountryPointsMetadata>
{
    /// <summary>
    ///     The required contest year filter value.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The required contest stage filter value.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The required competing country code filter value.
    /// </summary>
    public string CompetingCountryCode { get; init; } = string.Empty;

    public static CompetingCountryPointsMetadata CreateExample() =>
        new()
        {
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            CompetingCountryCode = "AT",
        };
}
