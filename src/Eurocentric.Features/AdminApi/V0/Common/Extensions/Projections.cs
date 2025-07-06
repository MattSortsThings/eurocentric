using System.Linq.Expressions;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using DomainContest = Eurocentric.Domain.PlaceholderEntities.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V0.Common.Contracts.Contest;

namespace Eurocentric.Features.AdminApi.V0.Common.Extensions;

internal static class Projections
{
    internal static Expression<Func<DomainContest, ContestDto>> ProjectToContestDto => contest => new ContestDto
    {
        Id = contest.Id,
        ContestYear = contest.ContestYear,
        CityName = contest.CityName,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat
    };
}
