using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Fluent builder for the <see cref="Country" /> class.
/// </summary>
public sealed class CountryBuilder
{
    private ErrorOr<CountryCode> _errorOrCountryCode = Error.Failure("Country code not provided.");
    private ErrorOr<CountryName> _errorOrName = Error.Failure("Name not provided.");

    internal CountryBuilder() { }

    /// <summary>
    ///     Sets the <see cref="Country.CountryCode" /> value of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorOrCountryCode">
    ///     The discriminated union of a legal <see cref="CountryCode" /> value or a list of
    ///     <see cref="Error" /> values. The success/error status is not evaluated until the <see cref="Build" /> method is
    ///     invoked.
    /// </param>
    /// <returns>The same <see cref="CountryBuilder" /> instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithCountryCode(ErrorOr<CountryCode> errorOrCountryCode)
    {
        _errorOrCountryCode = errorOrCountryCode;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Country.Name" /> value of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorOrCountryName">
    ///     The discriminated union of a legal <see cref="CountryName" /> value or a list of
    ///     <see cref="Error" /> values. The success/error status is not evaluated until the <see cref="Build" /> method is
    ///     invoked.
    /// </param>
    /// <returns>The same <see cref="CountryBuilder" /> instance, so that method invocations can be chained.</returns>
    public CountryBuilder WithName(ErrorOr<CountryName> errorOrCountryName)
    {
        _errorOrName = errorOrCountryName;

        return this;
    }

    /// <summary>
    ///     Attempts to build a new <see cref="Country" /> instance and returns either the successfully created instance or the
    ///     errors that arose.
    /// </summary>
    /// <param name="idProvider">Provides the <see cref="Country.Id" /> value for the returned instance.</param>
    /// <returns>
    ///     A new <see cref="Country" /> instance if it has been successfully built; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    public ErrorOr<Country> Build(Func<CountryId> idProvider)
    {
        ArgumentNullException.ThrowIfNull(idProvider);

        return (first: _errorOrCountryCode, second: _errorOrName)
            .Combine()
            .Then(tuple => new Country(idProvider(), tuple.First, tuple.Second));
    }
}
