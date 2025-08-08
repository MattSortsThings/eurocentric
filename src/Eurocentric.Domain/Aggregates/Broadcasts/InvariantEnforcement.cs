using ErrorOr;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Extension methods to enforce invariants across all existing <see cref="Broadcast" /> aggregates.
/// </summary>
public static class InvariantEnforcement
{
    /// <summary>
    ///     Fails if the newly instantiated <see cref="Broadcast" /> aggregate has the same
    ///     <see cref="Broadcast.BroadcastDate" /> value as an existing <see cref="Broadcast" /> aggregate.
    /// </summary>
    /// <param name="errorsOrBroadcast">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a
    ///     <see cref="Broadcast" /> object.
    /// </param>
    /// <param name="existingBroadcasts">All the existing <see cref="Broadcast" /> aggregates in the system.</param>
    /// <returns>
    ///     A new list of <see cref="Error" /> objects if the <paramref name="errorsOrBroadcast" /> argument is a
    ///     <see cref="Broadcast" /> and its <see cref="Broadcast.BroadcastDate" /> has the same value as a
    ///     <see cref="Broadcast" /> in <paramref name="existingBroadcasts" />; otherwise, the
    ///     <paramref name="errorsOrBroadcast" /> argument is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="existingBroadcasts" /> is <see langword="null" />.</exception>
    public static ErrorOr<Broadcast> FailOnBroadcastDateConflict(this ErrorOr<Broadcast> errorsOrBroadcast,
        IQueryable<Broadcast> existingBroadcasts)
    {
        ArgumentNullException.ThrowIfNull(existingBroadcasts);

        if (errorsOrBroadcast.IsError)
        {
            return errorsOrBroadcast;
        }

        BroadcastDate broadcastDate = errorsOrBroadcast.Value.BroadcastDate;

        return existingBroadcasts.Any(contest => contest.BroadcastDate == broadcastDate)
            ? BroadcastErrors.BroadcastDateConflict(broadcastDate)
            : errorsOrBroadcast;
    }
}
