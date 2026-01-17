using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Participant
{
    public Guid ParticipatingCountryId { get; init; }

    public SemiFinalDraw SemiFinalDraw { get; init; }

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;
}
