using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a jury in a broadcast.
/// </summary>
public sealed class Jury : Voter
{
    [UsedImplicitly(Reason = "EF Core")]
    private Jury() { }

    internal Jury(CountryId votingCountryId)
        : base(votingCountryId) { }

    private protected override void GivePointsAward(Competitor competitor, PointsValue pointsValue) =>
        competitor.ReceivePointsAward(new JuryAward(VotingCountryId, pointsValue));
}
