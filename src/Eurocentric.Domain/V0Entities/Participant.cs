using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record Participant
{
    public Guid ParticipatingCountryId { get; init; }

    public SemiFinalDraw SemiFinalDraw { get; init; }

    public string ActName { get; init; } = null!;

    public string SongTitle { get; init; } = null!;
}
