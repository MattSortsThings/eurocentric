using ErrorOr;
using Eurocentric.Domain.Enums;
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
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a contest aggregate
    ///     with a participant that references a non-existent country aggregate.
    /// </summary>
    /// <param name="countryId">The participating country ID.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error OrphanParticipatingCountryId(CountryId countryId) => Error.NotFound("Orphan participating country ID",
        "No country exists with the provided country ID.",
        new Dictionary<string, object> { { "countryId", countryId.Value } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a contest aggregate
    ///     with a multiple participants referencing the same country aggregate.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error DuplicateParticipatingCountryIds() => Error.Failure("Duplicate participating country IDs",
        "Each participant in a contest must have a participating country ID referencing a different country.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a Liverpool format
    ///     contest aggregate with illegal participant group sizes.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalLiverpoolFormatParticipantGroups() => Error.Failure(
        "Illegal Liverpool format participant groups",
        "A Liverpool format contest must have a single group 0 participant, at least three group 1 participants, " +
        "and at least three group 2 participants.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a Stockholm format
    ///     contest aggregate with illegal participant group sizes.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalStockholmFormatParticipantGroups() => Error.Failure(
        "Illegal Stockholm format participant groups",
        "A Stockholm format contest must have no group 0 participants, at least three group 1 participants, " +
        "and at least three group 2 participants.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a child broadcast
    ///     for a contest aggregate with the same contest stage as an existing child broadcast.
    /// </summary>
    /// <param name="contestStage">The contest stage.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error ChildBroadcastContestStageConflict(ContestStage contestStage) => Error.Conflict(
        "Child broadcast contest stage conflict",
        "A child broadcast already exists for the contest with the provided contest stage.",
        new Dictionary<string, object> { ["contestStage"] = contestStage.ToString() });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a child broadcast
    ///     for a contest aggregate with a date outside its parent contest's year.
    /// </summary>
    /// <param name="broadcastDate">The broadcast date.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error ChildBroadcastDateOutOfRange(DateOnly broadcastDate) => Error.Conflict(
        "Child broadcast date out of range",
        "A broadcast's date must be in the same year as its parent contest.",
        new Dictionary<string, object> { ["broadcastDate"] = broadcastDate.ToString("yyyy-MM-dd") });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a child broadcast
    ///     for a contest aggregate with a competing country ID that has no matching participant in the parent contest.
    /// </summary>
    /// <param name="countryId">The competing country ID.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error OrphanCompetingCountryId(CountryId countryId) => Error.Conflict(
        "Orphan competing country ID",
        "Competing country ID has no matching participant in parent contest.",
        new Dictionary<string, object> { ["countryId"] = countryId.Value });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a child broadcast
    ///     for a contest aggregate with a competing country ID that has no matching participant in the parent contest.
    /// </summary>
    /// <param name="countryId">The competing country ID.</param>
    /// <param name="contestStage">The contest stage.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IneligibleCompetingCountryId(CountryId countryId, ContestStage contestStage) =>
        Error.Conflict("Ineligible competing country ID",
            "Competing country ID matches ineligible participant in parent contest given contest stage.",
            new Dictionary<string, object> { ["countryId"] = countryId.Value, ["contestStage"] = contestStage.ToString() });
}
