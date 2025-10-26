using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest.
/// </summary>
public abstract class Contest : AggregateRoot<ContestId>
{
    private readonly List<ChildBroadcast> _childBroadcasts = [];
    private readonly List<Participant> _participants;

    [UsedImplicitly(Reason = "EF Core")]
    private protected Contest()
    {
        _participants = [];
    }

    private protected Contest(
        ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote? globalTelevote = null
    )
        : base(id)
    {
        _participants = participants;
        ContestYear = contestYear;
        CityName = cityName;
        GlobalTelevote = globalTelevote;
    }

    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public ContestYear ContestYear { get; private init; } = null!;

    /// <summary>
    ///     Gets the name of the contest's host city.
    /// </summary>
    public CityName CityName { get; private init; } = null!;

    /// <summary>
    ///     Gets the contest's rules.
    /// </summary>
    public abstract ContestRules ContestRules { get; private protected init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the contest is queryable.
    /// </summary>
    public bool Queryable { get; private set; }

    /// <summary>
    ///     Gets the contest's optional global televote.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; private init; }

    /// <summary>
    ///     Gets a list of all the contest's child broadcasts.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the contest's child broadcast list.</remarks>
    public IReadOnlyList<ChildBroadcast> ChildBroadcasts => _childBroadcasts.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the contest's participants.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the contest's participant list.</remarks>
    public IReadOnlyList<Participant> Participants => _participants.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets the contest's participants.
    /// </summary>
    /// <remarks>This internal property accesses the contest's participant list directly.</remarks>
    internal IReadOnlyCollection<Participant> ParticipantsCollection => _participants;

    private protected abstract class ContestBuilder : IContestBuilder
    {
        private protected Result<ContestYear, IDomainError> ErrorOrContestYear { get; private set; } =
            ContestErrors.ContestYearPropertyNotSet();

        private protected Result<CityName, IDomainError> ErrorOrCityName { get; private set; } =
            ContestErrors.CityNamePropertyNotSet();

        private protected GlobalTelevote? GlobalTelevote { get; private set; }

        private protected List<Result<Participant, IDomainError>> ErrorsOrParticipants { get; } = new(6);

        public IContestBuilder AddSemiFinal1Participant(
            CountryId countryId,
            Result<ActName, IDomainError> errorOrActName,
            Result<SongTitle, IDomainError> errorOrSongTitle
        )
        {
            ArgumentNullException.ThrowIfNull(countryId);

            ErrorsOrParticipants.Add(Participant.CreateInSemiFinal1(countryId, errorOrActName, errorOrSongTitle));

            return this;
        }

        public IContestBuilder AddSemiFinal2Participant(
            CountryId countryId,
            Result<ActName, IDomainError> errorOrActName,
            Result<SongTitle, IDomainError> errorOrSongTitle
        )
        {
            ArgumentNullException.ThrowIfNull(countryId);

            ErrorsOrParticipants.Add(Participant.CreateInSemiFinal2(countryId, errorOrActName, errorOrSongTitle));

            return this;
        }

        public IContestBuilder WithContestYear(Result<ContestYear, IDomainError> errorOrContestYear)
        {
            ErrorOrContestYear = errorOrContestYear;

            return this;
        }

        public IContestBuilder WithCityName(Result<CityName, IDomainError> errorOrCityName)
        {
            ErrorOrCityName = errorOrCityName;

            return this;
        }

        public IContestBuilder WithGlobalTelevote(CountryId countryId)
        {
            ArgumentNullException.ThrowIfNull(countryId);

            GlobalTelevote = new GlobalTelevote(countryId);

            return this;
        }

        public abstract Result<Contest, IDomainError> Build(Func<ContestId> idProvider);
    }
}
