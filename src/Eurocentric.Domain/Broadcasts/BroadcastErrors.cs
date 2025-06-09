using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

public static class BroadcastErrors
{
    public static Error BroadcastNotFound(BroadcastId broadcastId) => Error.NotFound("Broadcast not found",
        "No broadcast exists with the provided broadcast ID.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value });
}
