using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

public static class ContestErrors
{
    public static Error ContestNotFound(ContestId contestId) => Error.NotFound("Contest not found",
        "No contest exists with the provided contest ID.",
        new Dictionary<string, object> { { "contestId", contestId.Value } });

    public static Error ContestYearConflict(ContestYear contestYear) => Error.Conflict("Contest year conflict",
        "Contest already exists with the provided contest year.",
        new Dictionary<string, object> { { "contestYear", contestYear.Value } });

    public static Error DuplicateParticipatingCountryIds() => Error.Failure("Duplicate participating country IDs",
        "Every participant in a contest must have a different participating country ID.");

    public static Error OrphanParticipatingCountryIds(IEnumerable<CountryId> orphanCountryIds) =>
        Error.Conflict("Orphan participating country IDs",
            "Every participant in a contest must reference an existing country by its ID.",
            new Dictionary<string, object> { { "orphanCountryIds", orphanCountryIds.Select(id => id.Value).ToArray() } });

    public static Error IllegalLiverpoolFormatGroupSizes() => Error.Failure("Illegal Liverpool format group sizes",
        "A Liverpool format contest must have 1 Group 0 participant, " +
        "at least 3 Group 1 participants, and at least 3 Group 2 participants.");

    public static Error IllegalStockholmFormatGroupSizes() => Error.Failure("Illegal Stockholm format group sizes",
        "A Stockholm format contest must have no Group 0 participants, " +
        "at least 3 Group 1 participants, and at least 3 Group 2 participants.");
}
