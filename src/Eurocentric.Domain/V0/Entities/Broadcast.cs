using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Entities;

public sealed record Broadcast
{
    public Guid Id { get; init; }

    public DateOnly BroadcastDate { get; init; }

    public Guid ParentContestId { get; init; }

    public ContestStage ContestStage { get; init; }

    public bool Completed { get; init; }

    public List<Competitor> Competitors { get; init; } = [];

    public List<Jury> Juries { get; init; } = [];

    public List<Televote> Televotes { get; init; } = [];
}
