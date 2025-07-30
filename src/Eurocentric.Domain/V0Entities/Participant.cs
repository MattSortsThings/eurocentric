using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record Participant
{
    public required Guid ParticipatingCountryId { get; init; }

    public required ParticipantGroup ParticipantGroup { get; init; }

    public required string? ActName { get; init; }

    public required string? SongTitle { get; init; }
}
