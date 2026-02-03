using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Broadcast
{
    public required Guid Id { get; init; }

    public required DateOnly BroadcastDate { get; init; }

    public required Guid ParentContestId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required VotingFormat VotingFormat { get; init; }

    public required bool Completed { get; init; }

    public required List<Competitor> Competitors { get; init; }

    public required List<Televote> Televotes { get; init; }

    public required List<Jury> Juries { get; init; }
}
