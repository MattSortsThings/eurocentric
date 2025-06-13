using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

public static class BroadcastErrors
{
    public static Error BroadcastNotFound(BroadcastId broadcastId) => Error.NotFound("Broadcast not found",
        "No broadcast exists with the provided broadcast ID.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value });

    public static Error CannotDisqualify() => Error.Conflict("Cannot disqualify",
        "A competitor may only be disqualified when the broadcast status is Initialized.");

    public static Error CompetitorNotFound(BroadcastId broadcastId, CountryId competingCountryId) => Error.Conflict(
        "Competitor not found",
        "Broadcast has no competitor with the provided competing country ID.",
        new Dictionary<string, object>
        {
            ["broadcastId"] = broadcastId.Value, ["competingCountryId"] = competingCountryId.Value
        });

    public static Error InsufficientCompetitors() => Error.Failure("Insufficient competitors",
        "A broadcast must have at least 2 competitors.");

    public static Error DuplicateCompetingCountries() => Error.Failure("Duplicate competing countries",
        "Every competitor in a broadcast must reference a different competing country.");

    public static Error RankedCompetitorsMismatch() => Error.Conflict("Ranked competitors mismatch",
        "Ranked competing country IDs must contain every competing country ID in the broadcast " +
        "(excluding the voting country ID) exactly once.");

    public static Error JuryNotFound(BroadcastId broadcastId, CountryId votingCountryId) => Error.Conflict(
        "Jury not found",
        "Broadcast has no jury with the provided voting country ID.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value, ["votingCountryId"] = votingCountryId.Value });

    public static Error JuryPointsAlreadyAwarded(BroadcastId broadcastId, CountryId votingCountryId) => Error.Conflict(
        "Jury points already awarded",
        "Broadcast has a jury with the provided voting country ID, but it has already awarded its points.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value, ["votingCountryId"] = votingCountryId.Value });

    public static Error TelevoteNotFound(BroadcastId broadcastId, CountryId votingCountryId) => Error.Conflict(
        "Televote not found",
        "Broadcast has no televote with the provided voting country ID.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value, ["votingCountryId"] = votingCountryId.Value });

    public static Error TelevotePointsAlreadyAwarded(BroadcastId broadcastId, CountryId votingCountryId) => Error.Conflict(
        "Televote points already awarded",
        "Broadcast has a televote with the provided voting country ID, but it has already awarded its points.",
        new Dictionary<string, object> { ["broadcastId"] = broadcastId.Value, ["votingCountryId"] = votingCountryId.Value });
}
