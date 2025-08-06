using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

public sealed record CreateContestResponse(Contest Contest) : IExampleProvider<CreateContestResponse>
{
    public static CreateContestResponse CreateExample()
    {
        Contest contest = Contest.CreateExample() with
        {
            ChildBroadcasts = [],
            Participants =
            [
                new Participant { ParticipatingCountryId = ExampleValues.CountryId3Of3, ParticipantGroup = 0 },
                new Participant
                {
                    ParticipatingCountryId = ExampleValues.CountryId2Of3,
                    ParticipantGroup = 1,
                    ActName = "Lucio Corsi",
                    SongTitle = "Volevo Essere Un Duro"
                },
                Participant.CreateExample()
            ]
        };

        return new CreateContestResponse(contest);
    }
}
