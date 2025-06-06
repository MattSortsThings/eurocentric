using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Contests;

public static class ContestErrors
{
    public static Error ContestNotFound(ContestId contestId) => Error.NotFound("Contest not found",
        "No contest exists with the provided contest ID.",
        new Dictionary<string, object> { ["contestId"] = contestId.Value });
}
