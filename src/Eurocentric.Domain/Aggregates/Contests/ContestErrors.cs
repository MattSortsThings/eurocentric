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
}
