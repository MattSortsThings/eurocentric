using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record Broadcast
{
    public required Guid Id { get; init; }

    public required DateOnly BroadcastDate { get; init; }

    public required Guid ParentContestId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required bool Completed { get; init; }

    public required IList<Competitor> Competitors { get; init; }

    public required IList<Jury> Juries { get; init; }

    public required List<Televote> Televotes { get; init; }
}
