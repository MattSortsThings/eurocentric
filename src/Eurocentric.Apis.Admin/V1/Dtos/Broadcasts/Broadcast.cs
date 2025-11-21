using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a broadcast.
/// </summary>
public sealed record Broadcast : IDtoSchemaExampleProvider<Broadcast>
{
    /// <summary>
    ///     The broadcast's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The date on which the broadcast is televised.
    /// </summary>
    public DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The ID of the broadcast's parent contest.
    /// </summary>
    public Guid ParentContestId { get; init; }

    /// <summary>
    ///     The broadcast's contest stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the broadcast has been completed.
    /// </summary>
    public bool Completed { get; init; }

    /// <summary>
    ///     An array of all the broadcast's competitors.
    /// </summary>
    public Competitor[] Competitors { get; init; } = [];

    /// <summary>
    ///     An array of all the broadcast's juries.
    /// </summary>
    public Jury[] Juries { get; init; } = [];

    /// <summary>
    ///     An array of all the broadcast's televotes.
    /// </summary>
    public Televote[] Televotes { get; init; } = [];

    public static Broadcast CreateExample() =>
        new()
        {
            Id = V1ExampleIds.Broadcast,
            BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
            ParentContestId = V1ExampleIds.Contest,
            ContestStage = ContestStage.GrandFinal,
            Competitors = [Competitor.CreateExample()],
            Juries = [Jury.CreateExample()],
            Televotes = [Televote.CreateExample()],
        };

    public bool Equals(Broadcast? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id)
            && BroadcastDate.Equals(other.BroadcastDate)
            && ParentContestId.Equals(other.ParentContestId)
            && ContestStage == other.ContestStage
            && Completed == other.Completed
            && Competitors
                .OrderBy(competitor => competitor.CompetingCountryId)
                .SequenceEqual(other.Competitors.OrderBy(competitor => competitor.CompetingCountryId))
            && Juries
                .OrderBy(jury => jury.VotingCountryId)
                .SequenceEqual(other.Juries.OrderBy(jury => jury.VotingCountryId))
            && Televotes
                .OrderBy(televote => televote.VotingCountryId)
                .SequenceEqual(other.Televotes.OrderBy(vote => vote.VotingCountryId));
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            BroadcastDate,
            ParentContestId,
            (int)ContestStage,
            Completed,
            Competitors,
            Juries,
            Televotes
        );
}
