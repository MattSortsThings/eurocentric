using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Fluent builder for the <see cref="Broadcast" /> class.
/// </summary>
/// <remarks>
///     The client will work with a specific concrete derivative of this abstract base class, depending on the
///     concrete type of the parent contest instance and also on the contest stage of the broadcast to be created.
/// </remarks>
public abstract class BroadcastBuilder
{
    /// <summary>
    ///     Sets the <see cref="Broadcast.BroadcastDate" /> value of the <see cref="Broadcast" /> to be built.
    /// </summary>
    /// <param name="errorsOrBroadcastDate">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a <see cref="BroadcastDate" /> object.
    /// </param>
    /// <returns>The same <see cref="BroadcastBuilder" /> instance, so that method invocations can be chained.</returns>
    public abstract BroadcastBuilder WithBroadcastDate(ErrorOr<BroadcastDate> errorsOrBroadcastDate);

    /// <summary>
    ///     Sets the ordered <see cref="Broadcast.Competitors" /> collection of the <see cref="Broadcast" /> to be built.
    /// </summary>
    /// <remarks>
    ///     Invoking this method overwrites the results of any previous invocations of the same method on the same
    ///     <see cref="BroadcastBuilder" /> instance.
    /// </remarks>
    /// <param name="competingCountryIds">The IDs of the competing countries in the broadcast, in their running order.</param>
    /// <returns>The same <see cref="BroadcastBuilder" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="competingCountryIds" /> is <see langword="null" />.</exception>
    public abstract BroadcastBuilder WithCompetingCountryIds(IEnumerable<CountryId> competingCountryIds);

    /// <summary>
    ///     Creates and returns a new <see cref="Broadcast" /> instance as specified by the previous fluent builder method
    ///     invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     Provides the <see cref="Broadcast.Id" /> of the new <see cref="Broadcast" /> if it is successfully instantiated.
    /// </param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a new
    ///     <see cref="Broadcast" /> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="idProvider" /> is <see langword="null" />.</exception>
    public abstract ErrorOr<Broadcast> Build(Func<BroadcastId> idProvider);
}
