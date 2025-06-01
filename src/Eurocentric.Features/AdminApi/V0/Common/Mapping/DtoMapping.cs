using Eurocentric.Features.AdminApi.V0.Common.Enums;
using ContestDto = Eurocentric.Features.AdminApi.V0.Common.Dtos.Contest;
using PlaceholderContest = Eurocentric.Domain.Placeholders.Contest;

namespace Eurocentric.Features.AdminApi.V0.Common.Mapping;

internal static class DtoMapping
{
    internal static ContestDto ToContestDto(this PlaceholderContest contest) => new(contest.Id,
        contest.ContestYear,
        contest.CityName,
        Enum.Parse<ContestFormat>(contest.ContestFormat.ToString()));
}
