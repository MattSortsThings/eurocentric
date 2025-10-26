using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Fluent builder subset for the <see cref="Contest" /> class.
/// </summary>
public interface IContestParticipantAdder
{
    /// <summary>
    ///     Adds a <see cref="Participant" /> with a <see cref="SemiFinalDraw.SemiFinal1" /> draw to the
    ///     <see cref="Contest.Participants" /> list of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="countryId">The ID of the participating country.</param>
    /// <param name="errorOrActName"><i>Either</i> a legal <see cref="ActName" /> instance <i>or</i> an error.</param>
    /// <param name="errorOrSongTitle"><i>Either</i> a legal <see cref="SongTitle" /> instance <i>or</i> an error.</param>
    /// <returns>The same <see cref="IContestBuilder" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="countryId" /> is <see langword="null" />.</exception>
    IContestBuilder AddSemiFinal1Participant(
        CountryId countryId,
        Result<ActName, IDomainError> errorOrActName,
        Result<SongTitle, IDomainError> errorOrSongTitle
    );

    /// <summary>
    ///     Adds a <see cref="Participant" /> with a <see cref="SemiFinalDraw.SemiFinal2" /> draw to the
    ///     <see cref="Contest.Participants" /> list of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="countryId">The ID of the participating country.</param>
    /// <param name="errorOrActName"><i>Either</i> a legal <see cref="ActName" /> instance <i>or</i> an error.</param>
    /// <param name="errorOrSongTitle"><i>Either</i> a legal <see cref="SongTitle" /> instance <i>or</i> an error.</param>
    /// <returns>The same <see cref="IContestBuilder" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="countryId" /> is <see langword="null" />.</exception>
    IContestBuilder AddSemiFinal2Participant(
        CountryId countryId,
        Result<ActName, IDomainError> errorOrActName,
        Result<SongTitle, IDomainError> errorOrSongTitle
    );
}
