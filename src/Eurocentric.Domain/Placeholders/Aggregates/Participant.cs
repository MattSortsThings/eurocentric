using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Participant
{
    public required Guid ParticipatingCountryId { get; init; }

    public required SemiFinalDraw SemiFinalDraw { get; init; }

    public required string ActName { get; init; }

    public required string SongTitle { get; init; }
}
