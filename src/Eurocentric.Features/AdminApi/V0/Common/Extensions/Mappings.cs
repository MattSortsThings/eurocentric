using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using DomainContest = Eurocentric.Domain.PlaceholderEntities.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V0.Common.Contracts.Contest;

namespace Eurocentric.Features.AdminApi.V0.Common.Extensions;

internal static class Mappings
{
    internal static ContestDto ToContestDto(this DomainContest contest) => new()
    {
        Id = contest.Id,
        ContestYear = contest.ContestYear,
        CityName = contest.CityName,
        ContestFormat = Enum.Parse<ContestFormat>(contest.ContestFormat.ToString())
    };
}
