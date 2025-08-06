using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Factory methods to create common contest aggregate errors.
/// </summary>
public static class ContestErrors
{
    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the requested contest aggregate was not found.
    /// </summary>
    /// <param name="contestId">The requested contest ID.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error ContestNotFound(ContestId contestId) => Error.NotFound("Contest not found",
        "No contest exists with the provided contest ID.",
        new Dictionary<string, object> { { "contestId", contestId.Value } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a contest aggregate
    ///     with a non-unique contest year.
    /// </summary>
    /// <param name="contestYear">The contest year.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error ContestYearConflict(ContestYear contestYear) => Error.Conflict("Contest year conflict",
        "A contest already exists with the provided contest year.",
        new Dictionary<string, object> { { "contestYear", contestYear.Value } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a Liverpool format
    ///     contest aggregate with illegal participant group sizes.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalLiverpoolFormatParticipantGroups() => Error.Failure(
        "Illegal Liverpool format participant groups",
        "A Liverpool format contest must have one group 0 participant, " +
        "at least three group 1 participants, and at least three group 2 participants.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a Stockholm format
    ///     contest aggregate with illegal participant group sizes.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalStockholmFormatParticipantGroups() => Error.Failure(
        "Illegal Stockholm format participant groups",
        "A Stockholm format contest must have no group 0 participants, " +
        "at least three group 1 participants, and at least three group 2 participants.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a contest aggregate
    ///     with multiple participants referencing the same country aggregate.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error DuplicateParticipatingCountryIds() => Error.Failure(
        "Duplicate participating country IDs",
        "Every participant in a contest must have a different participating country ID.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to build a contest without
    ///     setting the contest year.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error ContestYearNotSet() => Error.Unexpected("Contest year not set",
        "Contest builder invoked without setting contest year.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to build a contest without
    ///     setting the city name.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error CityNameNotSet() => Error.Unexpected("City name not set",
        "Contest builder invoked without setting city name.");
}
