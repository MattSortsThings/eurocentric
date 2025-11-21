using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

/// <summary>
///     A single competitor points share rankings row.
/// </summary>
public sealed record CompetitorPointsShareRanking : IDtoSchemaExampleProvider<CompetitorPointsShareRanking>
{
    /// <summary>
    ///     The competitor's rank based on descending points share.
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
    ///     The sum total points the competitor received in the broadcast, as a fraction of the available points.
    /// </summary>
    public decimal PointsShare { get; init; }

    /// <summary>
    ///     The sum total points the competitor received in the broadcast.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The maximum available points the competitor could have received.
    /// </summary>
    public int AvailablePoints { get; init; }

    /// <summary>
    ///     The number of points awards in the queried filtered voting data for the competitor.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique voting countries in the queried filtered voting data for the competitor.
    /// </summary>
    public int VotingCountries { get; init; }

    public static CompetitorPointsShareRanking CreateExample() =>
        new()
        {
            Rank = 1,
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            CountryCode = "AA",
            CountryName = "CountryName",
            ActName = "ActName",
            SongTitle = "SongTitle",
            PointsShare = 0.5m,
            TotalPoints = 450,
            AvailablePoints = 900,
            PointsAwards = 75,
            VotingCountries = 38,
        };
}
