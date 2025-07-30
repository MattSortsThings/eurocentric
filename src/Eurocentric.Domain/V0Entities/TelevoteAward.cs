using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record TelevoteAward
{
    public required Guid VotingCountryId { get; init; }

    public required PointsValue PointsValue { get; init; }
}
