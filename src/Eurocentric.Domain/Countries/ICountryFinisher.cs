using ErrorOr;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Defines a method for the <see cref="Country" /> fluent builder.
/// </summary>
public interface ICountryFinisher
{
    /// <summary>
    ///     Creates and returns a valid <see cref="Country" /> instance from the previous fluent builder method invocations.
    /// </summary>
    /// <param name="dateTimeOffset">
    ///     Generates the underlying <see cref="Guid" /> value of the new instance's <see cref="Country.Id" /> property.
    /// </param>
    /// <returns>
    ///     A new <see cref="Country" /> instance if all business rules for an individual country aggregate are satisfied;
    ///     otherwise, a list of errors.
    /// </returns>
    public ErrorOr<Country> Build(DateTimeOffset dateTimeOffset);
}
