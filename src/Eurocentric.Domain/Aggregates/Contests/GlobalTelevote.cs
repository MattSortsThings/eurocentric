using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed class GlobalTelevote : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private GlobalTelevote()
    {
    }

    public GlobalTelevote(CountryId participatingCountryId)
    {
        ParticipatingCountryId = participatingCountryId;
    }

    /// <summary>
    ///     Gets the ID of the participating country.
    /// </summary>
    public CountryId ParticipatingCountryId { get; private init; } = null!;
}
