using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

public abstract class ContestBuilder
{
    private protected ErrorOr<ContestYear> ErrorsOrYear { get; private set; } = Error.Unexpected("Year not provided.");

    private protected ErrorOr<CityName> ErrorsOrCityName { get; private set; } = Error.Unexpected("City name not provided.");

    private protected List<ErrorOr<Participant>> ErrorsOrParticipants { get; } = new(6);

    public ContestBuilder WithYear(ErrorOr<ContestYear> errorsOrYear)
    {
        ErrorsOrYear = errorsOrYear;

        return this;
    }

    public ContestBuilder WithCityName(ErrorOr<CityName> errorsOrCityName)
    {
        ErrorsOrCityName = errorsOrCityName;

        return this;
    }

    public ContestBuilder WithGroupZeroParticipant(CountryId countryId)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroupZero(countryId));

        return this;
    }

    public ContestBuilder WithGroupOneParticipant(CountryId countryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroupOne(countryId, errorsOrActName, errorsOrSongTitle));

        return this;
    }

    public ContestBuilder WithGroupTwoParticipant(CountryId countryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroupTwo(countryId, errorsOrActName, errorsOrSongTitle));

        return this;
    }

    public abstract ErrorOr<Contest> Build(Func<ContestId> idProvider);
}
