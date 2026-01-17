using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class PointsAward
{
    public Guid VotingCountryId { get; init; }

    public VotingMethod VotingMethod { get; init; }

    public int PointsValue { get; init; }
}
