using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Fluent builder for the <see cref="Country" /> class.
/// </summary>
public sealed class CountryBuilder
{
    internal CountryBuilder() { }

    private ErrorOr<CountryCode> ErrorsOrCountryCode { get; set; } = Error.Unexpected("Country code not provided");

    private ErrorOr<CountryName> ErrorsOrCountryName { get; set; } = Error.Unexpected("Country name not provided");

    /// <summary>
    ///     Sets the <see cref="Country.CountryCode" /> value of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorsOrCountryCode">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects
    ///     <i>OR</i> a <see cref="CountryCode" /> value object.
    /// </param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryCode(ErrorOr<CountryCode> errorsOrCountryCode)
    {
        ErrorsOrCountryCode = errorsOrCountryCode;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Country.CountryName" /> value of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorsOrCountryName">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects
    ///     <i>OR</i> a <see cref="CountryName" /> value object.
    /// </param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryName(ErrorOr<CountryName> errorsOrCountryName)
    {
        ErrorsOrCountryName = errorsOrCountryName;

        return this;
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Country" /> instance as specified by the previous fluent builder method
    ///     invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     Provides the <see cref="Country.Id" /> of the new <see cref="Country" /> if it is successfully
    ///     instantiated.
    /// </param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a new
    ///     <see cref="Country" /> object.
    /// </returns>
    public ErrorOr<Country> Build(Func<CountryId> idProvider) => Tuple.Create(ErrorsOrCountryCode, ErrorsOrCountryName)
        .Combine()
        .Then(tuple => new Country(idProvider(), tuple.Item1, tuple.Item2));
}
