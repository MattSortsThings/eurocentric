using System.Data;
using Dapper;
using Eurocentric.Domain.Analytics.Rankings.Common;

namespace Eurocentric.Components.Gateways;

internal sealed class RankingsDynamicParameters : DynamicParameters
{
    internal static RankingsDynamicParameters From<T>(T query)
        where T : class
    {
        RankingsDynamicParameters dp = new();

        if (query is IOptionalBroadcastFiltering ob)
        {
            dp.PopulateFrom(ob);
        }

        if (query is IOptionalCompetingCountryFiltering occ)
        {
            dp.PopulateFrom(occ);
        }

        if (query is IOptionalPaginationOverrides op)
        {
            dp.PopulateFrom(op);
        }

        if (query is IOptionalVotingCountryFiltering ovc)
        {
            dp.PopulateFrom(ovc);
        }

        if (query is IOptionalVotingMethodFiltering ovm)
        {
            dp.PopulateFrom(ovm);
        }

        if (query is IRequiredCompetingCountryFiltering rcc)
        {
            dp.PopulateFrom(rcc);
        }

        return dp;
    }

    private void PopulateFrom(IOptionalBroadcastFiltering filtering)
    {
        if (filtering.MinYear is { } minYear)
        {
            Add("@min_year", minYear, DbType.Int32, ParameterDirection.Input);
        }

        if (filtering.MaxYear is { } maxYear)
        {
            Add("@max_year", maxYear, DbType.Int32, ParameterDirection.Input);
        }

        if (filtering.ContestStage is { } contestStage)
        {
            Add(
                "@contest_stage",
                contestStage.ToString(),
                DbType.String,
                size: 10,
                direction: ParameterDirection.Input
            );
        }
    }

    private void PopulateFrom(IOptionalPaginationOverrides overrides)
    {
        if (overrides.PageIndex is { } pageIndex)
        {
            Add("@page_index", pageIndex, DbType.Int32, ParameterDirection.Input);
        }

        if (overrides.PageSize is { } pageSize)
        {
            Add("@page_size", pageSize, DbType.Int32, ParameterDirection.Input);
        }

        if (overrides.Descending is { } descending)
        {
            Add("@descending", descending, DbType.Boolean, ParameterDirection.Input);
        }
    }

    private void PopulateFrom(IOptionalCompetingCountryFiltering filtering)
    {
        if (filtering.CompetingCountryCode is { } competingCountryCode)
        {
            Add(
                "@competing_country_code",
                competingCountryCode,
                DbType.StringFixedLength,
                size: 2,
                direction: ParameterDirection.Input
            );
        }
    }

    private void PopulateFrom(IOptionalVotingCountryFiltering filtering)
    {
        if (filtering.VotingCountryCode is { } votingCountryCode)
        {
            Add(
                "@voting_country_code",
                votingCountryCode,
                DbType.StringFixedLength,
                size: 2,
                direction: ParameterDirection.Input
            );
        }
    }

    private void PopulateFrom(IOptionalVotingMethodFiltering filtering)
    {
        if (filtering.VotingMethod is { } votingMethod)
        {
            Add(
                "@voting_method",
                votingMethod.ToString(),
                DbType.String,
                size: 10,
                direction: ParameterDirection.Input
            );
        }
    }

    private void PopulateFrom(IRequiredCompetingCountryFiltering filtering)
    {
        Add(
            "@competing_country_code",
            filtering.CompetingCountryCode,
            DbType.StringFixedLength,
            size: 2,
            direction: ParameterDirection.Input
        );
    }
}
