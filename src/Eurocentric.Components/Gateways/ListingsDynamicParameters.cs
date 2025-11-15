using System.Data;
using Dapper;
using Eurocentric.Domain.Analytics.Listings;

namespace Eurocentric.Components.Gateways;

internal sealed class ListingsDynamicParameters : DynamicParameters
{
    internal static ListingsDynamicParameters From<T>(T query)
        where T : class
    {
        ListingsDynamicParameters dp = new();

        Populate(dp, query);

        return dp;
    }

    private static void Populate<T>(ListingsDynamicParameters dp, T query)
        where T : class
    {
        if (query is IRequiredVotingCountryFiltering rvc)
        {
            dp.PopulateFrom(rvc);
        }

        if (query is IRequiredBroadcastFiltering rb)
        {
            dp.PopulateFrom(rb);
        }

        if (query is IRequiredCompetingCountryFiltering rcc)
        {
            dp.PopulateFrom(rcc);
        }
    }

    private void PopulateFrom(IRequiredBroadcastFiltering filtering)
    {
        Add("@contest_year", filtering.ContestYear, DbType.Int32, ParameterDirection.Input);

        Add(
            "@contest_stage",
            filtering.ContestStage.ToString(),
            DbType.String,
            size: 10,
            direction: ParameterDirection.Input
        );
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

    private void PopulateFrom(IRequiredVotingCountryFiltering filtering)
    {
        Add(
            "@voting_country_code",
            filtering.VotingCountryCode,
            DbType.StringFixedLength,
            size: 2,
            direction: ParameterDirection.Input
        );
    }
}
