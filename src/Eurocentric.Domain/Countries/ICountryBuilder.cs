namespace Eurocentric.Domain.Countries;

/// <summary>
///     Defines a method for the <see cref="Country" /> fluent builder.
/// </summary>
public interface ICountryBuilder
{
    /// <summary>
    ///     Specifies the country code value for the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="countryCode">A string of 2 upper-case letters. The country's ISO 3166-1 alpha-2 country code.</param>
    /// <returns>The same fluent builder, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="countryCode" /> is <see langword="null" />.</exception>
    public ICountryNameSetter WithCountryCode(string countryCode);
}
