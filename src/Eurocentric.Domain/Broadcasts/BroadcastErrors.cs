using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

public static class BroadcastErrors
{
    public static Error BroadcastNotFound(BroadcastId broadcastId) => Error.NotFound("Broadcast not found",
        "No broadcast exists with the provided broadcast ID.",
        new Dictionary<string, object> { { "broadcastId", broadcastId.Value } });

    public static Error IllegalBroadcastSize() => Error.Failure("Illegal broadcast size",
        "Broadcast must have at least 2 competitors.");

    public static Error DuplicateCompetingCountryIds() => Error.Failure("Duplicate competing country IDs",
        "Every competitor in a broadcast must have a different competing country ID.");

    public static Error IllegalCompetingCountryIds(IEnumerable<CountryId> competingCountryIds, ContestStage contestStage) =>
        Error.Conflict("Illegal competing country IDs",
            "Every competitor in a broadcast must share a country ID with a contest participant " +
            "eligible to compete in the requested contest stage.",
            new Dictionary<string, object>
            {
                { "illegalCompetingCountryIds", competingCountryIds.Select(id => id.Value).ToArray() },
                { "contestStage", contestStage.ToString() }
            });
}
