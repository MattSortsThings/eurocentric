using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record Participant
{
    public Guid ParticipatingCountryId { get; init; }

    public ParticipantGroup ParticipantGroup { get; init; }

    public string? ActName { get; init; }

    public string? SongTitle { get; init; }
}
