using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

public abstract class ContestBuilder
{
    private ErrorOr<ContestYear> ErrorsOrContestYear { get; set; } = Error.Unexpected("Contest year not provided.");

    private ErrorOr<CityName> ErrorsOrCityName { get; set; } = Error.Unexpected("City name not provided.");

    private List<ErrorOr<Participant>> ErrorsOrParticipants { get; } = new(7);

    public ContestBuilder WithContestYear(ErrorOr<ContestYear> errorsOrContestYear)
    {
        ErrorsOrContestYear = errorsOrContestYear;

        return this;
    }

    public ContestBuilder WithCityName(ErrorOr<CityName> errorsOrCityName)
    {
        ErrorsOrCityName = errorsOrCityName;

        return this;
    }

    public ContestBuilder AddGroup0Participant(CountryId countryId)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroup0(countryId));

        return this;
    }

    public ContestBuilder AddGroup1Participant(CountryId countryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroup1(countryId, errorsOrActName, errorsOrSongTitle));

        return this;
    }

    public ContestBuilder AddGroup2Participant(CountryId countryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroup2(countryId, errorsOrActName, errorsOrSongTitle));

        return this;
    }

    public ErrorOr<Contest> Build(IContestIdGenerator idGenerator)
    {
        ArgumentNullException.ThrowIfNull(idGenerator);

        return Tuple.Create(ErrorsOrContestYear, ErrorsOrCityName, CollectAndCheck(ErrorsOrParticipants))
            .Combine()
            .Then(tuple => InitializeContest(idGenerator.Generate(), tuple.Item1, tuple.Item2, tuple.Item3));
    }

    private protected abstract ErrorOr<List<Participant>> CollectAndCheck(
        IReadOnlyList<ErrorOr<Participant>> errorsOrParticipants);

    private protected abstract Contest InitializeContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants);
}
