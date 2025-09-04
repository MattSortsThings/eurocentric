using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

public sealed class Participant : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private Participant()
    {
    }

    public Participant(CountryId participatingCountryId, SemiFinalDraw semiFinalDraw, ActName actName, SongTitle songTitle)
    {
        ParticipatingCountryId = participatingCountryId;
        SemiFinalDraw = semiFinalDraw;
        ActName = actName;
        SongTitle = songTitle;
    }

    public CountryId ParticipatingCountryId { get; private init; } = null!;

    public SemiFinalDraw SemiFinalDraw { get; private init; }

    public ActName ActName { get; private init; } = null!;

    public SongTitle SongTitle { get; private init; } = null!;
}
