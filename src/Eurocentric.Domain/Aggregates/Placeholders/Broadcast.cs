using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Broadcast
{
    public Guid Id { get; init; }

    public DateOnly BroadcastDate { get; init; }

    public Guid ParentContestId { get; init; }

    public ContestStage ContestStage { get; init; }

    public VotingRules VotingRules { get; init; }

    public bool Completed { get; init; }

    public List<Competitor> Competitors { get; init; } = [];

    public List<Televote> Televotes { get; init; } = [];

    public List<Jury> Juries { get; init; } = [];
}
