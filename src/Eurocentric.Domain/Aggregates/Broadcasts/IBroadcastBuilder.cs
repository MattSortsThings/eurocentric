using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Fluent builder for the <see cref="Broadcast" /> class.
/// </summary>
public interface IBroadcastBuilder
{
    /// <summary>
    ///     Sets the <see cref="Broadcast.BroadcastDate" /> property of the <see cref="Broadcast" /> to be built.
    /// </summary>
    /// <param name="errorOrBroadcastDate"><i>Either</i> a legal <see cref="BroadcastDate" /> instance <i>or</i> an error.</param>
    /// <returns>The same <see cref="IBroadcastBuilder" /> instance, so that method invocations can be chained.</returns>
    IBroadcastBuilder WithBroadcastDate(Result<BroadcastDate, IDomainError> errorOrBroadcastDate);

    /// <summary>
    ///     Sets the  <see cref="Broadcast.Competitors" /> property contents of the <see cref="Broadcast" /> to be built.
    /// </summary>
    /// <param name="competingCountryIds">
    ///     The IDs of the competing countries, in broadcast running order, with vacant running order spots represented by
    ///     <see langword="null" /> values.
    /// </param>
    /// <returns>The same <see cref="IBroadcastBuilder" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="competingCountryIds" /> is <see langword="null" />.</exception>
    IBroadcastBuilder WithCompetingCountries(params CountryId?[] competingCountryIds);

    /// <summary>
    ///     Builds a new <see cref="Broadcast" /> aggregate based on the previous method invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     A function that provides the <see cref="Broadcast.Id" /> value of the new <see cref="Broadcast" />
    ///     instance. This function is only invoked if the build is successful.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="Broadcast" /> instance <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="idProvider" /> is <see langword="null" />.</exception>
    Result<Broadcast, IDomainError> Build(Func<BroadcastId> idProvider);
}
