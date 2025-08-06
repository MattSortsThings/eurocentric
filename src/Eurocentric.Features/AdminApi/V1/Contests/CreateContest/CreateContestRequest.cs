using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

public sealed record CreateContestRequest : IExampleProvider<CreateContestRequest>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public Guid? Group0ParticipatingCountryId { get; init; }

    public required ContestParticipantDatum[] Group1ParticipantData { get; init; }

    public required ContestParticipantDatum[] Group2ParticipantData { get; init; }

    public static CreateContestRequest CreateExample() => new()
    {
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        Group0ParticipatingCountryId = ExampleValues.CountryId3Of3,
        Group1ParticipantData =
        [
            new ContestParticipantDatum
            {
                ParticipatingCountryId = ExampleValues.CountryId2Of3,
                ActName = "Lucio Corsi",
                SongTitle = "Volevo Essere Un Duro"
            }
        ],
        Group2ParticipantData = [ContestParticipantDatum.CreateExample()]
    };
}
