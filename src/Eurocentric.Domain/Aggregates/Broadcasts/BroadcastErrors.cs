using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Factory methods to create common broadcast aggregate errors.
/// </summary>
public static class BroadcastErrors
{
    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the requested broadcast aggregate was not found.
    /// </summary>
    /// <param name="broadcastId">The requested broadcast ID.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error BroadcastNotFound(BroadcastId broadcastId) => Error.NotFound("Broadcast not found",
        "No broadcast exists with the provided broadcast ID.",
        new Dictionary<string, object> { { "broadcastId", broadcastId.Value } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a broadcast
    ///     aggregate with a non-unique broadcast date.
    /// </summary>
    /// <param name="broadcastDate">The broadcast date.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error BroadcastDateConflict(BroadcastDate broadcastDate) => Error.Conflict("Broadcast date conflict",
        "A broadcast already exists with the provided broadcast date.",
        new Dictionary<string, object> { { "broadcastDate", broadcastDate.Value.ToString("yyyy-MM-dd") } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a broadcast
    ///     aggregate with competitors representing the same country.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error DuplicateCompetingCountryIds() => Error.Failure("Duplicate competing country IDs",
        "Every competitor in a broadcast must have a different competing country ID.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a broadcast
    ///     aggregate with fewer than 2 competitors.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCompetitorCount() => Error.Failure("Illegal competitor count",
        "A broadcast must have at least 2 competitors.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to build a broadcast without
    ///     setting the broadcast date.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error BroadcastDateNotSet() => Error.Unexpected("Broadcast date not set",
        "Broadcast builder invoked without setting broadcast date.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to build a broadcast without
    ///     setting the competitors.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error CompetitorsNotSet() => Error.Unexpected("Competitors not set",
        "Broadcast builder invoked without setting competitors.");
}
