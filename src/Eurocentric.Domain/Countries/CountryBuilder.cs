using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Fluent builder for the <see cref="Country" /> class.
/// </summary>
public sealed class CountryBuilder
{
    internal CountryBuilder() { }

    private ErrorOr<CountryCode> ErrorsOrCountryCode { get; set; } = Error.Unexpected("Country code not provided");

    private ErrorOr<CountryName> ErrorsOrCountryName { get; set; } = Error.Unexpected("Country name not provided");

    /// <summary>
    ///     Sets the <see cref="Country.CountryCode" /> property for the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorsOrCountryCode">
    ///     The discriminated union of a list of <see cref="Error" /> values or a <see cref="CountryCode" /> value.
    /// </param>
    /// <returns>The same <see cref="CountryBuilder" /> instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryCode(ErrorOr<CountryCode> errorsOrCountryCode)
    {
        ErrorsOrCountryCode = errorsOrCountryCode;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Country.CountryName" /> property for the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorsOrCountryName">
    ///     The discriminated union of a list of <see cref="Error" /> values or a <see cref="CountryName" /> value.
    /// </param>
    /// <returns>The same <see cref="CountryBuilder" /> instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryName(ErrorOr<CountryName> errorsOrCountryName)
    {
        ErrorsOrCountryName = errorsOrCountryName;

        return this;
    }

    /// <summary>
    ///     Builds a new <see cref="Country" /> instance as specified from the previous fluent builder method invocations.
    /// </summary>
    /// <param name="idGenerator">
    ///     Generates the <see cref="Country.Id" /> property for the new <see cref="Country" /> instance.
    /// </param>
    /// <returns>
    ///     A new <see cref="Country" /> instance if a legal instance was built from the previous fluent builder method
    ///     invocations; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="idGenerator" /> is <see langword="null" />.</exception>
    public ErrorOr<Country> Build(ICountryIdGenerator idGenerator)
    {
        ArgumentNullException.ThrowIfNull(idGenerator);

        return Tuple.Create(ErrorsOrCountryCode, ErrorsOrCountryName)
            .Combine()
            .Then(tuple => new Country(idGenerator.Generate(), tuple.Item1, tuple.Item2));
    }
}
