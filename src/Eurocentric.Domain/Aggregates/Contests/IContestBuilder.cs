using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Fluent builder for the <see cref="Contest" /> class.
/// </summary>
public interface IContestBuilder : IContestParticipantAdder
{
    /// <summary>
    ///     Sets the <see cref="Contest.ContestYear" /> property of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="errorOrContestYear"><i>Either</i> a legal <see cref="ContestYear" /> instance <i>or</i> an error.</param>
    /// <returns>The same <see cref="IContestBuilder" /> instance, so that method invocations can be chained.</returns>
    IContestBuilder WithContestYear(Result<ContestYear, IDomainError> errorOrContestYear);

    /// <summary>
    ///     Sets the <see cref="Contest.CityName" /> property of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="errorOrCityName"><i>Either</i> a legal <see cref="CityName" /> instance <i>or</i> an error.</param>
    /// <returns>The same <see cref="IContestBuilder" /> instance, so that method invocations can be chained.</returns>
    IContestBuilder WithCityName(Result<CityName, IDomainError> errorOrCityName);

    /// <summary>
    ///     Sets the <see cref="Contest.GlobalTelevote" /> property of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <remarks>If this method is not invoked, the <see cref="Contest.GlobalTelevote" /> property is <see langword="null" />.</remarks>
    /// <param name="countryId">The ID of the voting country.</param>
    /// <returns>The same <see cref="IContestBuilder" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="countryId" /> is <see langword="null" />.</exception>
    IContestBuilder WithGlobalTelevote(CountryId countryId);

    /// <summary>
    ///     Builds a new <see cref="Contest" /> aggregate based on the previous method invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     A function that provides the <see cref="Contest.Id" /> value of the new <see cref="Contest" />
    ///     instance. This function is only invoked if the build is successful.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="Contest" /> instance <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="idProvider" /> is <see langword="null" />.</exception>
    Result<Contest, IDomainError> Build(Func<ContestId> idProvider);
}
