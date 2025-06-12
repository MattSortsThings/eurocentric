using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a televote that awards a set of points in a broadcast.
/// </summary>
public sealed class Televote : Voter
{
    private Televote()
    {
    }

    public Televote(CountryId votingCountryId) : base(votingCountryId)
    {
    }

    private protected override void AwardPoints(Competitor competitor, PointsValue pointsValue) =>
        competitor.ReceiveAward(new TelevoteAward(VotingCountryId, pointsValue));
}
