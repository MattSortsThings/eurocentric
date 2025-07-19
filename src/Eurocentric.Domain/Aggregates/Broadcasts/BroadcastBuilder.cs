using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Fluent builder for the <see cref="Broadcast" /> class.
/// </summary>
public abstract class BroadcastBuilder
{
    private protected ErrorOr<DateOnly> ErrorsOrBroadcastDate { get; set; } =
        Error.Unexpected("Broadcast date not provided");

    private protected ErrorOr<CountryId[]> ErrorsOrCompetingCountryIds { get; set; } =
        Error.Unexpected("Competing country IDs not provided");

    /// <summary>
    ///     Sets the <see cref="Broadcast.BroadcastDate" /> value of the <see cref="Broadcast" /> to be built.
    /// </summary>
    /// <param name="broadcastDate">The broadcast's transmission date.</param>
    /// <returns>The same <see cref="BroadcastBuilder" /> instance, so that method invocations can be chained.</returns>
    public BroadcastBuilder WithBroadcastDate(DateOnly broadcastDate)
    {
        ErrorsOrBroadcastDate = broadcastDate;

        return this;
    }

    /// <summary>
    ///     Sets the ordered <see cref="Broadcast.Competitors" /> of the <see cref="Broadcast" /> to be built.
    /// </summary>
    /// <param name="competingCountryIds">The competing country IDs in broadcast running order.</param>
    /// <returns>The same <see cref="BroadcastBuilder" /> instance, so that method invocations can be chained.</returns>
    public BroadcastBuilder WithCompetingCountryIds(IEnumerable<CountryId> competingCountryIds)
    {
        ArgumentNullException.ThrowIfNull(competingCountryIds);

        ErrorsOrCompetingCountryIds = competingCountryIds.ToArray();

        return this;
    }

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
