using System.ComponentModel;
using System.Data;
using Dapper;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0Analytics.Rankings.Common;

namespace Eurocentric.Infrastructure.AnalyticsGateways.V0.Rankings;

internal sealed class RankingsDynamicParameters : DynamicParameters
{
    internal void AddPaginationParametersFrom(IPaginatedQuery query)
    {
        Add("@page_index", query.PageIndex, DbType.Int32, ParameterDirection.Input);
        Add("@page_size", query.PageSize, DbType.Int32, ParameterDirection.Input);
        Add("@descending", query.Descending, DbType.Boolean, ParameterDirection.Input);
        Add("@total_rankings", dbType: DbType.Int32, direction: ParameterDirection.Output);
    }

    internal void AddContestStagesParameterFrom(IContestStagesFilter filter)
    {
        DataTable tvp = new();
        tvp.Columns.Add("value", typeof(int));

        foreach (ContestStage value in filter.ContestStages ?? Enum.GetValues<ContestStage>())
        {
            tvp.Rows.Add((int)value);
        }

        Add("@contest_stages", tvp, DbType.Object, ParameterDirection.Input);
    }

    internal void AddOptionalYearRangeParametersFrom(IYearRangeFilter filter)
    {
        if (filter.MinYear is { } minYear)
        {
            Add("@min_year", minYear, DbType.Int32, ParameterDirection.Input);
        }

        if (filter.MaxYear is { } maxYear)
        {
            Add("@max_year", maxYear, DbType.Int32, ParameterDirection.Input);
        }
    }

    internal void AddVotingMethodParametersFrom(IVotingMethodFilter filter)
    {
        switch (filter.VotingMethod)
        {
            case VotingMethod.Jury:
                Add("@exclude_jury", false, DbType.Boolean, ParameterDirection.Input);
                Add("@exclude_televote", true, DbType.Boolean, ParameterDirection.Input);

                break;
            case VotingMethod.Televote:
                Add("@exclude_jury", true, DbType.Boolean, ParameterDirection.Input);
                Add("@exclude_televote", false, DbType.Boolean, ParameterDirection.Input);

                break;
            case VotingMethod.Any:
            case null:
                Add("@exclude_jury", false, DbType.Boolean, ParameterDirection.Input);
                Add("@exclude_televote", false, DbType.Boolean, ParameterDirection.Input);

                break;
            default:
                throw new InvalidEnumArgumentException(nameof(filter.VotingMethod), (int)filter.VotingMethod,
                    typeof(VotingMethod));
        }
    }

    internal void AddOptionalVotingCountryParameterFrom(IVotingCountryFilter filter)
    {
        if (filter.VotingCountryCode is { } votingCountryCode)
        {
            Add("@voting_country_code", votingCountryCode, DbType.String, ParameterDirection.Input, 2);
        }
    }

    internal void AddPointsRangeParametersFrom(IPointsInRangeQuery query)
    {
        Add("@min_points", query.MinPoints, DbType.Int32, ParameterDirection.Input);
        Add("@max_points", query.MaxPoints, DbType.Int32, ParameterDirection.Input);
    }

    internal RankingsTotals GetRankingsTotals()
    {
        int totalRankings = Get<int>("@total_rankings");
        int pageSize = Get<int>("@page_size");
        int totalPages = (int)Math.Ceiling(totalRankings / (double)pageSize);

        return new RankingsTotals(totalPages, totalRankings);
    }
}
