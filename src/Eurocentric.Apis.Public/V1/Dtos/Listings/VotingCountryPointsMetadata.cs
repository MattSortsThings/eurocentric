using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     Metadata describing an executed voting country points listings query.
/// </summary>
public sealed record VotingCountryPointsMetadata : IDtoSchemaExampleProvider<VotingCountryPointsMetadata>
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
    ///     The required voting country code filter value.
    /// </summary>
    public string VotingCountryCode { get; init; } = string.Empty;

    public static VotingCountryPointsMetadata CreateExample() =>
        new()
        {
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            VotingCountryCode = "AT",
        };
}
