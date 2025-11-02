using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Domain errors that may occur when working with <see cref="Broadcast" /> aggregates.
/// </summary>
public static class BroadcastErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has requested a <see cref="Broadcast" /> that
    ///     does not exist in the system.
    /// </summary>
    /// <param name="broadcastId">The ID of the requested country.</param>
    /// <returns>A new <see cref="NotFoundError" /> instance.</returns>
    public static NotFoundError BroadcastNotFound(BroadcastId broadcastId)
    {
        return new NotFoundError
        {
            Title = "Broadcast not found",
            Detail = "The requested broadcast does not exist.",
            Extensions = new Dictionary<string, object?> { { nameof(broadcastId), broadcastId.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Broadcast" />
    ///     with a non-unique <see cref="Broadcast.BroadcastDate" />.
    /// </summary>
    /// <param name="broadcastDate">The broadcast date.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError BroadcastDateConflict(BroadcastDate broadcastDate)
    {
        return new ConflictError
        {
            Title = "Broadcast date conflict",
            Detail = "A broadcast already exists with the provided broadcast date.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(broadcastDate), broadcastDate.Value.ToString("yyyy-MM-dd") },
            },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to award a set of jury points
    ///     providing a voting country ID that matches no jury in the requested <see cref="Broadcast" /> that has not yet
    ///     awarded points.
    /// </summary>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <param name="countryId">The country ID.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError JuryVotingCountryConflict(BroadcastId broadcastId, CountryId countryId)
    {
        return new ConflictError
        {
            Title = "Jury voting country conflict",
            Detail = "The requested broadcast has no jury that may award points and has the provided country ID.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(broadcastId), broadcastId.Value },
                { nameof(countryId), countryId.Value },
            },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to award a set of televote points
    ///     providing a voting country ID that matches no televote in the requested <see cref="Broadcast" /> that has not yet
    ///     awarded points.
    /// </summary>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <param name="countryId">The country ID.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError TelevoteVotingCountryConflict(BroadcastId broadcastId, CountryId countryId)
    {
        return new ConflictError
        {
            Title = "Televote voting country conflict",
            Detail = "The requested broadcast has no televote that may award points and has the provided country ID.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(broadcastId), broadcastId.Value },
                { nameof(countryId), countryId.Value },
            },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to award a set of points providing a
    ///     voting country ID and an ordered array of competing country IDs that do not match the expected competing country
    ///     IDs in the requested <see cref="Broadcast" />.
    /// </summary>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError RankedCompetingCountriesConflict(BroadcastId broadcastId)
    {
        return new ConflictError
        {
            Title = "Ranked competing countries conflict",
            Detail =
                "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication.",
            Extensions = new Dictionary<string, object?> { { nameof(broadcastId), broadcastId.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Broadcast" /> in
    ///     which one of its <see cref="Broadcast.Competitors" /> references a country that matches no participant in the
    ///     parent <see cref="Contest" /> eligible to compete in its <see cref="Broadcast.ContestStage" />.
    /// </summary>
    /// <param name="contestId">The contest ID.</param>
    /// <param name="contestStage">The contest stage.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError ParentContestChildBroadcastsConflict(ContestId contestId, ContestStage contestStage)
    {
        return new ConflictError
        {
            Title = "Parent contest child broadcasts conflict",
            Detail = "The requested contest already has a child broadcast with the provided contest stage.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(contestId), contestId.Value },
                { nameof(contestStage), contestStage.ToString() },
            },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Broadcast" /> with
    ///     a <see cref="Broadcast.BroadcastDate" /> value that is outside the year of its parent <see cref="Contest" />.
    /// </summary>
    /// <param name="contestId">The contest ID.</param>
    /// <param name="broadcastDate">The broadcast date.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError ParentContestYearConflict(ContestId contestId, BroadcastDate broadcastDate)
    {
        return new ConflictError
        {
            Title = "Parent contest year conflict",
            Detail = "The requested contest's year does not match the provided broadcast date.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(contestId), contestId.Value },
                { nameof(broadcastDate), broadcastDate.Value.ToString("yyyy-MM-dd") },
            },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Broadcast" /> with
    ///     a <see cref="Broadcast.ContestStage" /> value equal to an existing child broadcast for its parent
    ///     <see cref="Contest" />.
    /// </summary>
    /// <param name="contestId">The contest ID.</param>
    /// <param name="contestStage">The contest stage.</param>
    /// <param name="countryId">The country ID.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError ParentContestParticipantsConflict(
        ContestId contestId,
        ContestStage contestStage,
        CountryId countryId
    )
    {
        return new ConflictError
        {
            Title = "Parent contest participants conflict",
            Detail =
                "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(contestId), contestId.Value },
                { nameof(contestStage), contestStage.ToString() },
                { nameof(countryId), countryId.Value },
            },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Broadcast" />
    ///     with one or more <see cref="Broadcast.Competitors" /> referencing the same country.
    /// </summary>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalCompetingCountries()
    {
        return new UnprocessableError
        {
            Title = "Illegal competing countries",
            Detail = "Each competitor in a broadcast must reference a different country.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Broadcast" /> with an
    ///     insufficient quantity of <see cref="Broadcast.Competitors" />.
    /// </summary>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalCompetitorsCount()
    {
        return new UnprocessableError
        {
            Title = "Illegal competitors count",
            Detail = "A broadcast must have at least 2 competitors.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Broadcast" />
    ///     without setting its <see cref="Broadcast.BroadcastDate" /> property.
    /// </summary>
    /// <returns>A new <see cref="UnexpectedError" /> instance.</returns>
    public static UnexpectedError BroadcastDatePropertyNotSet()
    {
        return new UnexpectedError
        {
            Title = "BroadcastDate property not set",
            Detail = "Client attempted to create a Broadcast aggregate without setting its BroadcastDate property.",
            Extensions = null,
        };
    }
}
