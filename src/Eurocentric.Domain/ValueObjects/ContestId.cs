using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Uniquely identifies a contest in the system.
/// </summary>
public sealed class ContestId : GuidAtomicValueObject
{
    private ContestId(Guid value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with the provided
    ///     <see cref="GuidAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">The underlying <see cref="Guid" /> value of the instance to be created.</param>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public static ContestId FromValue(Guid value) => new(value);
}
