using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Fluent builder for the <see cref="Country" /> class.
/// </summary>
public interface ICountryBuilder
{
    /// <summary>
    ///     Sets the <see cref="Country.CountryCode" /> property of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorOrCountryCode">
    ///     <i>Either</i> a legal <see cref="CountryCode" /> instance <i>or</i> an error.
    /// </param>
    /// <returns>The same <see cref="ICountryBuilder" /> instance, so that method invocations can be chained.</returns>
    ICountryBuilder WithCountryCode(Result<CountryCode, IDomainError> errorOrCountryCode);

    /// <summary>
    ///     Sets the <see cref="Country.CountryName" /> property of the <see cref="Country" /> to be built.
    /// </summary>
    /// <param name="errorOrCountryName">
    ///     <i>Either</i> a legal <see cref="CountryName" /> instance <i>or</i> an error.
    /// </param>
    /// <returns>The same <see cref="ICountryBuilder" /> instance, so that method invocations can be chained.</returns>
    ICountryBuilder WithCountryName(Result<CountryName, IDomainError> errorOrCountryName);

    /// <summary>
    ///     Builds a new <see cref="Country" /> aggregate based on the previous method invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     A function that provides the <see cref="Country.Id" /> value of the new <see cref="Country" />
    ///     instance. This function is only invoked if the build is successful.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="Country" /> instance <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="idProvider" /> is <see langword="null" />.</exception>
    Result<Country, IDomainError> Build(Func<CountryId> idProvider);
}
