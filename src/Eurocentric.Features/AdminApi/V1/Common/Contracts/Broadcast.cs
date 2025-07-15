using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record Broadcast : IExampleProvider<Broadcast>
{
    /// <summary>
    ///     The broadcast's unique ID.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     The broadcast's transmission date.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The broadcast parent contest's ID.
    /// </summary>
    public required Guid ParentContestId { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The broadcast's status.
    /// </summary>
    public required BroadcastStatus BroadcastStatus { get; init; }

    /// <summary>
    ///     The broadcast's competitors.
    /// </summary>
    public required Competitor[] Competitors { get; init; }

    /// <summary>
    ///     The broadcast's juries.
    /// </summary>
    public required Voter[] Juries { get; init; }

    /// <summary>
    ///     The broadcast's televotes.
    /// </summary>
    public required Voter[] Televotes { get; init; }

    public static Broadcast CreateExample() => new()
    {
        Id = ExampleIds.Broadcast,
        ParentContestId = ExampleIds.Contest,
        ContestStage = ContestStage.GrandFinal,
        BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
        BroadcastStatus = BroadcastStatus.InProgress,
        Competitors =
        [
            new Competitor
            {
                FinishingPosition = 1,
                RunningOrderPosition = 1,
                CompetingCountryId = ExampleIds.CountryAt,
                JuryAwards =
                [
                    new PointsAward { VotingCountryId = ExampleIds.CountryIt, PointsValue = 12 }
                ],
                TelevoteAwards =
                [
                    new PointsAward { VotingCountryId = ExampleIds.CountryIt, PointsValue = 0 }
                ]
            }
        ],
        Juries =
        [
            new Voter { VotingCountryId = ExampleIds.CountryIt, PointsAwarded = true }
        ],
        Televotes =
        [
            new Voter { VotingCountryId = ExampleIds.CountryIt, PointsAwarded = true }
        ]
    };
}
