using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents the global televote in a contest.
/// </summary>
public sealed class GlobalTelevote : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private GlobalTelevote() { }

    internal GlobalTelevote(CountryId votingCountryId)
    {
        VotingCountryId = votingCountryId;
    }

    /// <summary>
    ///     Gets the ID of the global televote's voting country.
    /// </summary>
    public CountryId VotingCountryId { get; private init; } = null!;
}
