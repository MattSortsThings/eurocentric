using System.ComponentModel;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.PublicApi.V0.Common.Enums;

namespace Eurocentric.Features.PublicApi.V0.Common.QueryHelpers;

internal static class QueryableExtensions
{
    internal static IQueryable<QueryablePointsAward> FilterByCompetingCountryCode(this IQueryable<QueryablePointsAward> awards,
        string countryCode) => awards.Where(award => award.CompetingCountryCode == countryCode);

    internal static IQueryable<QueryablePointsAward> FilterByStartYear(this IQueryable<QueryablePointsAward> awards,
        int? startYear) => startYear is not null ? awards.Where(award => award.ContestYear >= startYear) : awards;

    internal static IQueryable<QueryablePointsAward> FilterByEndYear(this IQueryable<QueryablePointsAward> awards,
        int? endYear) => endYear is not null ? awards.Where(award => award.ContestYear <= endYear) : awards;

    internal static IQueryable<QueryablePointsAward> FilterByContestStages(this IQueryable<QueryablePointsAward> awards,
        ContestStages contestStages) => contestStages switch
    {
        ContestStages.All => awards,
        ContestStages.SemiFinal1 => awards.Where(award => award.ContestStage == ContestStage.SemiFinal1),
        ContestStages.SemiFinal2 => awards.Where(award => award.ContestStage == ContestStage.SemiFinal2),
        ContestStages.SemiFinals => awards.Where(award =>
            award.ContestStage == ContestStage.SemiFinal1 || award.ContestStage == ContestStage.SemiFinal2),
        ContestStages.GrandFinal => awards.Where(award => award.ContestStage == ContestStage.GrandFinal),
        _ => throw new InvalidEnumArgumentException(nameof(contestStages), (int)contestStages, typeof(ContestStages))
    };
}
