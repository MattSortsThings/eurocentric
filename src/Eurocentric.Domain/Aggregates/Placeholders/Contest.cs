using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Contest
{
    public Guid Id { get; init; }

    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public VotingRules SemiFinalVotingRules { get; init; }

    public VotingRules GrandFinalVotingRules { get; init; }

    public bool Queryable { get; init; }

    public GlobalTelevote? GlobalTelevote { get; init; }

    public List<BroadcastMemo> BroadcastMemos { get; init; } = [];

    public List<Participant> Participants { get; init; } = [];
}
