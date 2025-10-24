using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Uniquely identifies a country in the system.
/// </summary>
public sealed class CountryId : GuidAtomicValueObject
{
    private CountryId(Guid value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with the provided
    ///     <see cref="GuidAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">The underlying <see cref="Guid" /> value of the instance to be created.</param>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public static CountryId FromValue(Guid value) => new(value);
}
