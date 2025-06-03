using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using PlaceholderContest = Eurocentric.Domain.Placeholders.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

public static class PlaceholderContestExtensions
{
    public static Contest ToContestDto(this PlaceholderContest contest) => new(contest.Id,
        contest.ContestYear,
        contest.CityName,
        Enum.Parse<ContestFormat>(contest.ContestFormat.ToString()));
}
