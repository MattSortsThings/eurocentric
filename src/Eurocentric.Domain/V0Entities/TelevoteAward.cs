using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record TelevoteAward
{
    public Guid VotingCountryId { get; init; }

    public PointsValue PointsValue { get; init; }
}
