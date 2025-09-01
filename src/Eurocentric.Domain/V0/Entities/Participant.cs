using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Entities;

public sealed record Participant
{
    public Guid ParticipatingCountryId { get; init; }

    public SemiFinalDraw SemiFinalDraw { get; init; }

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;
}
