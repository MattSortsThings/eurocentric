using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Uniquely identifies a broadcast in the system.
/// </summary>
public sealed class BroadcastId : GuidAtomicValueObject
{
    private BroadcastId(Guid value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastId" /> instance with the provided
    ///     <see cref="GuidAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">The underlying <see cref="Guid" /> value of the instance to be created.</param>
    /// <returns>A new <see cref="BroadcastId" /> instance.</returns>
    public static BroadcastId FromValue(Guid value) => new(value);
}
