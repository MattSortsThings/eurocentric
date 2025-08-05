using ErrorOr;
using Eurocentric.Domain.Identifiers;

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
}
