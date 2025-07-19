using ErrorOr;
using Eurocentric.Domain.Identifiers;

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
    ///     aggregate
    ///     with a multiple competitors referencing the same country aggregate.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error DuplicateCompetingCountryIds() => Error.Failure("Duplicate competing country IDs",
        "Each competitor in a broadcast must have a competing country ID referencing a different country.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a broadcast
    ///     aggregate
    ///     with an insufficient number of competitors.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalBroadcastSize() => Error.Failure("Illegal broadcast size",
        "A broadcast must have at least 2 competitors.");
}
