using Eurocentric.Features.Shared.Documentation;
using ExampleValues = Eurocentric.Features.AdminApi.V1.Common.Constants.ExampleValues;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

public sealed record ContestParticipantDatum : IExampleProvider<ContestParticipantDatum>
{
    public required Guid ParticipatingCountryId { get; init; }

    public required string ActName { get; init; }

    public required string SongTitle { get; init; }

    public static ContestParticipantDatum CreateExample() => new()
    {
        ParticipatingCountryId = ExampleValues.CountryId1Of3, ActName = "JJ", SongTitle = "Wasted Love"
    };

    public void Deconstruct(out Guid participatingCountryId, out string actName, out string songTitle)
    {
        participatingCountryId = ParticipatingCountryId;
        actName = ActName;
        songTitle = SongTitle;
    }
}
