namespace Eurocentric.Domain.Countries;

/// <summary>
///     Defines a method for the <see cref="Country" /> fluent builder.
/// </summary>
public interface ICountryNameSetter
{
    /// <summary>
    ///     Specifies the country name value for the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="countryName">
    ///     A non-empty, non-white-space string of no more than 200 characters. The country's short UK English name.
    /// </param>
    /// <returns>The same fluent builder, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="countryName" /> is <see langword="null" />.</exception>
    public ICountryFinisher AndCountryName(string countryName);
}
