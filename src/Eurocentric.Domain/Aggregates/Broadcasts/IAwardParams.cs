using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

public interface IAwardParams
{
    CountryId VotingCountryId { get; }

    IReadOnlyList<CountryId> RankedCompetingCountryIds { get; }
}
