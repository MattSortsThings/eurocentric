using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a jury that awards a set of points in a broadcast.
/// </summary>
public sealed class Jury : Voter
{
    private Jury()
    {
    }

    public Jury(CountryId votingCountryId) : base(votingCountryId)
    {
    }

    private protected override void AwardPoints(Competitor competitor, PointsValue pointsValue) =>
        competitor.ReceiveAward(new JuryAward(VotingCountryId, pointsValue));
}
