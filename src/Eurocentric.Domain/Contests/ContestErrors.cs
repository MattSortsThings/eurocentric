using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

public static class ContestErrors
{
    public static Error ContestNotFound(ContestId contestId) => Error.NotFound("Contest not found",
        "No contest exists with the provided contest ID.",
        new Dictionary<string, object> { ["contestId"] = contestId.Value });

    public static Error ContestYearConflict(ContestYear contestYear) => Error.Conflict("Contest year conflict",
        "A contest already exists with the provided contest year.",
        new Dictionary<string, object> { ["contestYear"] = contestYear.Value });

    public static Error OrphanParticipant(CountryId participatingCountryId) => Error.Conflict("Orphan participant",
        "No country exists with the provided participating country ID.",
        new Dictionary<string, object> { ["participatingCountryId"] = participatingCountryId.Value });

    public static Error DuplicateParticipatingCountries() => Error.Failure("Duplicate participating countries",
        "Every participant in a contest must reference a different participating country.");

    public static Error IllegalLiverpoolFormatGroupSizes() => Error.Failure("Illegal Liverpool format group sizes",
        "A Liverpool format contest must have a single participant in group 0, " +
        "at least 3 in group 1, and at least 3 in group 2.");

    public static Error IllegalStockholmFormatGroupSizes() => Error.Failure("Illegal Stockholm format group sizes",
        "A Stockholm format contest must have no participants in group 0, " +
        "at least 3 in group 1, and at least 3 in group 2.");
}
