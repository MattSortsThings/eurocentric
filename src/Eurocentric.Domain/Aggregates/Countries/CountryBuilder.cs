using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Fluent builder for the <see cref="Country" /> class.
/// </summary>
public sealed class CountryBuilder
{
    internal CountryBuilder() { }

    private ErrorOr<CountryCode> ErrorsOrCountryCode { get; set; } = CountryErrors.CountryCodeNotSet();

    private ErrorOr<CountryName> ErrorsOrCountryName { get; set; } = CountryErrors.CountryNameNotSet();

    /// <summary>
    ///     Sets the <see cref="Country.CountryCode" /> of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorsOrCountryCode">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects
    ///     <i>OR</i> a <see cref="CountryCode" /> object.
    /// </param>
    /// <returns>The same <see cref="CountryBuilder" /> instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryCode(ErrorOr<CountryCode> errorsOrCountryCode)
    {
        ErrorsOrCountryCode = errorsOrCountryCode;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Country.CountryName" /> of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorsOrCountryName">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects
    ///     <i>OR</i> a <see cref="CountryName" /> object.
    /// </param>
    /// <returns>The same <see cref="CountryBuilder" /> instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryName(ErrorOr<CountryName> errorsOrCountryName)
    {
        ErrorsOrCountryName = errorsOrCountryName;

        return this;
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Country" /> as specified by the previous builder method invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     Provides the <see cref="Country.Id" /> of the instantiated <see cref="Country" />. This
    ///     delegate is only invoked if a legal <see cref="Country" /> can be instantiated.
    /// </param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a new
    ///     <see cref="Country" /> object.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="idProvider" /> is <see langword="null" />.</exception>
    public ErrorOr<Country> Build(Func<CountryId> idProvider)
    {
        ArgumentNullException.ThrowIfNull(idProvider);

        return Tuple.Create(ErrorsOrCountryCode, ErrorsOrCountryName)
            .Combine()
            .Then(tuple => new Country(idProvider(), tuple.Item1, tuple.Item2));
    }
}
