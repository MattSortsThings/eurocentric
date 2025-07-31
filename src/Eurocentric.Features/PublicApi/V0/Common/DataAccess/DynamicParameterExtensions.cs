using System.ComponentModel;
using System.Data;
using Dapper;
using Eurocentric.Features.PublicApi.V0.Common.Enums;

namespace Eurocentric.Features.PublicApi.V0.Common.DataAccess;

internal static class DynamicParameterExtensions
{
    internal static int GetTotalItemsOutputParamValue(this DynamicParameters dynamicParameters) =>
        dynamicParameters.Get<int>("@total_items");

    internal static DynamicParameters AddPaginationInputParams(this DynamicParameters dynamicParameters,
        int pageIndex,
        int pageSize,
        bool descending)
    {
        dynamicParameters.Add("@page_index", pageIndex, DbType.Int32, ParameterDirection.Input);
        dynamicParameters.Add("@page_size", pageSize, DbType.Int32, ParameterDirection.Input);
        dynamicParameters.Add("@descending", descending, DbType.Boolean, ParameterDirection.Input);

        return dynamicParameters;
    }

    internal static DynamicParameters AddTotalItemsOutputParam(this DynamicParameters dynamicParameters)
    {
        dynamicParameters.Add("@total_items", dbType: DbType.Int32, direction: ParameterDirection.Output);

        return dynamicParameters;
    }

    internal static DynamicParameters AddVotingMethodInputParams(this DynamicParameters dynamicParameters,
        QueryableVotingMethod votingMethod)
    {
        switch (votingMethod)
        {
            case QueryableVotingMethod.Any:
                dynamicParameters.Add("@exclude_jury", false, DbType.Boolean, ParameterDirection.Input);
                dynamicParameters.Add("@exclude_televote", false, DbType.Boolean, ParameterDirection.Input);

                break;
            case QueryableVotingMethod.Jury:
                dynamicParameters.Add("@exclude_jury", false, DbType.Boolean, ParameterDirection.Input);
                dynamicParameters.Add("@exclude_televote", true, DbType.Boolean, ParameterDirection.Input);

                break;
            case QueryableVotingMethod.Televote:
                dynamicParameters.Add("@exclude_jury", true, DbType.Boolean, ParameterDirection.Input);
                dynamicParameters.Add("@exclude_televote", false, DbType.Boolean, ParameterDirection.Input);

                break;
            default:
                throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(QueryableVotingMethod));
        }

        return dynamicParameters;
    }

    internal static DynamicParameters AddContestStagesInputParams(this DynamicParameters dynamicParameters,
        QueryableContestStage contestStage)
    {
        DataTable tvp = new();
        tvp.Columns.Add("value", typeof(int));

        switch (contestStage)
        {
            case QueryableContestStage.Any:
                tvp.Rows.Add((int)ContestStage.SemiFinal1);
                tvp.Rows.Add((int)ContestStage.SemiFinal2);
                tvp.Rows.Add((int)ContestStage.GrandFinal);

                break;
            case QueryableContestStage.SemiFinal1:
                tvp.Rows.Add((int)ContestStage.SemiFinal1);

                break;
            case QueryableContestStage.SemiFinal2:
                tvp.Rows.Add((int)ContestStage.SemiFinal2);

                break;
            case QueryableContestStage.SemiFinals:
                tvp.Rows.Add((int)ContestStage.SemiFinal1);
                tvp.Rows.Add((int)ContestStage.SemiFinal2);

                break;
            case QueryableContestStage.GrandFinal:
                tvp.Rows.Add((int)ContestStage.GrandFinal);

                break;
            default:
                throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(QueryableContestStage));
        }

        dynamicParameters.Add("@contest_stages", tvp, DbType.Object, ParameterDirection.Input);

        return dynamicParameters;
    }

    internal static DynamicParameters AddContestYearInputParams(this DynamicParameters dynamicParameters,
        int? minYear,
        int? maxYear)
    {
        if (minYear is { } min)
        {
            dynamicParameters.Add("@min_year", min, DbType.Int32, ParameterDirection.Input);
        }

        if (maxYear is { } max)
        {
            dynamicParameters.Add("@max_year", max, DbType.Int32, ParameterDirection.Input);
        }

        return dynamicParameters;
    }

    internal static DynamicParameters AddPointsValueInputParams(this DynamicParameters dynamicParameters,
        int minPoints,
        int maxPoints)
    {
        dynamicParameters.Add("@min_points", minPoints, DbType.Int32, ParameterDirection.Input);
        dynamicParameters.Add("@max_points", maxPoints, DbType.Int32, ParameterDirection.Input);

        return dynamicParameters;
    }

    internal static DynamicParameters AddVotingCountryCodeInputParam(this DynamicParameters dynamicParameters,
        string? votingCountryCode)
    {
        if (votingCountryCode is not null)
        {
            dynamicParameters.Add("@voting_country_code", votingCountryCode, DbType.String, ParameterDirection.Input);
        }

        return dynamicParameters;
    }
}
