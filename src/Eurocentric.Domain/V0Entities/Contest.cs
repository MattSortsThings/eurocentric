using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public abstract record Contest
{
    public Guid Id { get; init; }

    public int ContestYear { get; init; }

    public string CityName { get; init; } = null!;

    public abstract ContestRules ContestRules { get; init; }

    public bool Queryable { get; set; }

    public List<ChildBroadcast> ChildBroadcasts { get; init; } = [];

    public List<Participant> Participants { get; init; } = [];

    public GlobalTelevote? GlobalTelevote { get; init; }
}
