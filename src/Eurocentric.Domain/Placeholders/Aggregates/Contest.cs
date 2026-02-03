using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Contest
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required VotingFormat SemiFinalVotingFormat { get; init; }

    public required VotingFormat GrandFinalVotingFormat { get; init; }

    public required bool Queryable { get; init; }

    public required List<BroadcastMemo> BroadcastMemos { get; init; }

    public GlobalTelevote? GlobalTelevote { get; init; }

    public required List<Participant> Participants { get; init; }
}
