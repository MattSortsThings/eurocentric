using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

public static class BroadcastErrors
{
    public static Error BroadcastNotFound(BroadcastId broadcastId) => Error.NotFound("Broadcast not found",
        "No broadcast exists with the provided broadcast ID.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value });

    public static Error InsufficientCompetitors() => Error.Failure("Insufficient competitors",
        "A broadcast must have at least 2 competitors.");

    public static Error DuplicateCompetingCountries() => Error.Failure("Duplicate competing countries",
        "Every competitor in a broadcast must reference a different competing country.");
}
