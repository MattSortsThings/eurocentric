using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Queryables;

/// <summary>
///     A queryable contest.
/// </summary>
/// <remarks>A contest is queryable once all three of its child broadcasts have been created and completed.</remarks>
public sealed record QueryableContest : IDtoSchemaExampleProvider<QueryableContest>
{
    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The UK English name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     The number of participants in the contest.
    /// </summary>
    public int Participants { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the contest uses the "Rest of the World" televote.
    /// </summary>
    public bool UsesRestOfWorldTelevote { get; init; }

    public static QueryableContest CreateExample()
    {
        return new QueryableContest
        {
            ContestYear = 2025,
            CityName = "Basel",
            Participants = 37,
            UsesRestOfWorldTelevote = true,
        };
    }

    public bool Equals(QueryableContest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ContestYear == other.ContestYear
            && CityName == other.CityName
            && Participants == other.Participants
            && UsesRestOfWorldTelevote == other.UsesRestOfWorldTelevote;
    }

    public override int GetHashCode() => HashCode.Combine(ContestYear, CityName, Participants, UsesRestOfWorldTelevote);
}
