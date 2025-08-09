using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts.Utils;

public sealed class VoterMatcherBuilder
{
    private readonly List<Func<Voter, bool>> _predicates = [];

    public VoterMatcherBuilder HasVotingCountryId(CountryId countryId)
    {
        _predicates.Add(voter => voter.VotingCountryId == countryId);

        return this;
    }

    public VoterMatcherBuilder HasPointsAwarded(bool value)
    {
        _predicates.Add(voter => voter.PointsAwarded == value);

        return this;
    }

    public Func<Voter, bool> Build() => voter => _predicates.All(predicate => predicate(voter));
}
