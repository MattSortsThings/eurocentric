using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Generates a <see cref="BroadcastId" /> instance on demand.
/// </summary>
public interface IBroadcastIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastId" /> instance with a random <see cref="BroadcastId.Value" />.
    /// </summary>
    /// <returns>A new <see cref="BroadcastId" /> instance.</returns>
    public BroadcastId Generate();
}
