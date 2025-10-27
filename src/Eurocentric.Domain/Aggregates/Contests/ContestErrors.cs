using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Domain errors that may occur when working with <see cref="Contest" /> aggregates.
/// </summary>
public static class ContestErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has requested a <see cref="Contest" /> that does not
    ///     exist in the system.
    /// </summary>
    /// <param name="contestId">The ID of the requested country.</param>
    /// <returns>A new <see cref="NotFoundError" /> instance.</returns>
    public static NotFoundError ContestNotFound(ContestId contestId)
    {
        return new NotFoundError
        {
            Title = "Contest not found",
            Detail = "The requested contest does not exist.",
            Extensions = new Dictionary<string, object?> { { nameof(contestId), contestId.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Contest" /> in which
    ///     any of its <see cref="Contest.Participants" /> or <see cref="Contest.GlobalTelevote" /> references a non-existent
    ///     country.
    /// </summary>
    /// <param name="countryId">The country ID.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError OrphanContestCountry(CountryId countryId)
    {
        return new ConflictError
        {
            Title = "Orphan contest country",
            Detail = "Every participant and global televote in a contest must reference an existing country.",
            Extensions = new Dictionary<string, object?> { { nameof(countryId), countryId.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Contest" /> with a
    ///     non-unique <see cref="Contest.ContestYear" />.
    /// </summary>
    /// <param name="contestYear">The contest year.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError ContestYearConflict(ContestYear contestYear)
    {
        return new ConflictError
        {
            Title = "Contest year conflict",
            Detail = "A contest already exists with the provided contest year.",
            Extensions = new Dictionary<string, object?> { { nameof(contestYear), contestYear.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Contest" /> with one or
    ///     more <see cref="Contest.Participants" /> and <see cref="Contest.GlobalTelevote" /> referencing the same country.
    /// </summary>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalContestCountries()
    {
        return new UnprocessableError
        {
            Title = "Illegal contest countries",
            Detail = "Each participant and global televote in a contest must reference a different country.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Contest" /> with an
    ///     illegal <see cref="Contest.GlobalTelevote" /> given its <see cref="Contest.ContestRules" />.
    /// </summary>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalGlobalTelevote()
    {
        return new UnprocessableError
        {
            Title = "Illegal global televote",
            Detail =
                "A Liverpool rules contest must have a global televote. "
                + "A Stockholm rules contest must not have a global televote.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Contest" /> with
    ///     illegal <see cref="Contest.Participants" /> counts.
    /// </summary>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalParticipantCounts()
    {
        return new UnprocessableError
        {
            Title = "Illegal participant counts",
            Detail = "A contest must have at least 3 participants for each semi-final draw.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Contest" />
    ///     without setting its <see cref="Contest.ContestYear" /> property.
    /// </summary>
    /// <returns>A new <see cref="UnexpectedError" /> instance.</returns>
    public static UnexpectedError ContestYearPropertyNotSet()
    {
        return new UnexpectedError
        {
            Title = "ContestYear property not set",
            Detail = "Client attempted to create a Contest aggregate without setting its ContestYear property.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Contest" />
    ///     without setting its <see cref="Contest.CityName" /> property.
    /// </summary>
    /// <returns>A new <see cref="UnexpectedError" /> instance.</returns>
    public static UnexpectedError CityNamePropertyNotSet()
    {
        return new UnexpectedError
        {
            Title = "CityName property not set",
            Detail = "Client attempted to create a Contest aggregate without setting its CityName property.",
            Extensions = null,
        };
    }
}
