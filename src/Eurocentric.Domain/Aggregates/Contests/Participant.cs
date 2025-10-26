using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Entity = Eurocentric.Domain.Core.Entity;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed class Participant : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private Participant() { }

    internal Participant(
        CountryId participatingCountryId,
        SemiFinalDraw semiFinalDraw,
        ActName actName,
        SongTitle songTitle
    )
    {
        ParticipatingCountryId = participatingCountryId;
        SemiFinalDraw = semiFinalDraw;
        ActName = actName;
        SongTitle = songTitle;
    }

    /// <summary>
    ///     Gets the ID of the participating country.
    /// </summary>
    public CountryId ParticipatingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the participant's semi-final draw.
    /// </summary>
    public SemiFinalDraw SemiFinalDraw { get; private init; }

    /// <summary>
    ///     Gets the participant's act name.
    /// </summary>
    public ActName ActName { get; private init; } = null!;

    /// <summary>
    ///     Gets the participant's song title.
    /// </summary>
    public SongTitle SongTitle { get; private init; } = null!;

    internal static Result<Participant, IDomainError> CreateInSemiFinal1(
        CountryId countryId,
        Result<ActName, IDomainError> errorOrActName,
        Result<SongTitle, IDomainError> errorOrSongTitle
    ) => ValueTuple.Create(errorOrActName, errorOrSongTitle).Combine().Map(DrawSemiFinal1(countryId));

    internal static Result<Participant, IDomainError> CreateInSemiFinal2(
        CountryId countryId,
        Result<ActName, IDomainError> errorOrActName,
        Result<SongTitle, IDomainError> errorOrSongTitle
    ) => ValueTuple.Create(errorOrActName, errorOrSongTitle).Combine().Map(DrawSemiFinal2(countryId));

    private static Func<ValueTuple<ActName, SongTitle>, Participant> DrawSemiFinal1(CountryId countryId)
    {
        CountryId id = countryId;

        return tuple => new Participant(id, SemiFinalDraw.SemiFinal1, tuple.Item1, tuple.Item2);
    }

    private static Func<ValueTuple<ActName, SongTitle>, Participant> DrawSemiFinal2(CountryId countryId)
    {
        CountryId id = countryId;

        return tuple => new Participant(id, SemiFinalDraw.SemiFinal2, tuple.Item1, tuple.Item2);
    }
}
