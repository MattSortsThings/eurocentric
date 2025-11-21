using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

/// <summary>
///     A single competitor points in range rankings row.
/// </summary>
public sealed record CompetitorPointsInRangeRanking : IDtoSchemaExampleProvider<CompetitorPointsInRangeRanking>
{
    /// <summary>
    ///     The competitor's rank based on descending points in range.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    ///     The contest year of the broadcast.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The broadcast's stage in the contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The competitor's running order spot in the broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's finishing position in the broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     The competitor's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;

    /// <summary>
    ///     The frequency of points awards within the specified value range the competitor received in their broadcast,
    ///     relative to the number of points awards the competitor received.
    /// </summary>
    public decimal PointsInRange { get; init; }

    /// <summary>
    ///     The frequency of points awards within the specified value range the competitor received in their broadcast.
    /// </summary>
    public int PointsAwardsInRange { get; init; }

    /// <summary>
    ///     The number of points awards in the queried filtered voting data for the competitor.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique voting countries in the queried filtered voting data for the competitor.
    /// </summary>
    public int VotingCountries { get; init; }

    public static CompetitorPointsInRangeRanking CreateExample() =>
        new()
        {
            Rank = 1,
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            CountryCode = "AA",
            CountryName = "CountryName",
            ActName = "ActName",
            SongTitle = "SongTitle",
            PointsInRange = 0.666667m,
            PointsAwardsInRange = 50,
            PointsAwards = 75,
            VotingCountries = 38,
        };
}
