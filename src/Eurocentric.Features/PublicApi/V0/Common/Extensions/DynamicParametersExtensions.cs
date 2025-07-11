using System.ComponentModel;
using System.Data;
using Dapper;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using ContestStage = Eurocentric.Features.AdminApi.V1.Common.Contracts.ContestStage;

namespace Eurocentric.Features.PublicApi.V0.Common.Extensions;

internal static class DynamicParametersExtensions
{
    internal static DynamicParameters AddVotingMethodInputParameters(this DynamicParameters dp, VotingMethodFilter votingMethod)
    {
        switch (votingMethod)
        {
            case VotingMethodFilter.Any:
                dp.Add("@include_televote_awards", true, DbType.Boolean, ParameterDirection.Input);
                dp.Add("@include_jury_awards", true, DbType.Boolean, ParameterDirection.Input);

                break;
            case VotingMethodFilter.Jury:
                dp.Add("@include_jury_awards", true, DbType.Boolean, ParameterDirection.Input);

                break;
            case VotingMethodFilter.Televote:
                dp.Add("@include_televote_awards", true, DbType.Boolean, ParameterDirection.Input);

                break;
            default:
                throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(VotingMethodFilter));
        }

        return dp;
    }

    internal static DynamicParameters AddContestYearInputParameters(
        this DynamicParameters dp,
        int? minYear = null,
        int? maxYear = null)
    {
        if (minYear is { } min)
        {
            dp.Add("@min_contest_year", min, DbType.Int32, ParameterDirection.Input);
        }

        if (maxYear is { } max)
        {
            dp.Add("@max_contest_year", max, DbType.Int32, ParameterDirection.Input);
        }

        return dp;
    }

    internal static DynamicParameters AddVotingCountryCodeInputParameter(
        this DynamicParameters dp,
        string? votingCountryCode = null)
    {
        if (votingCountryCode is not null)
        {
            dp.Add("@voting_country_code", votingCountryCode, DbType.String, ParameterDirection.Input);
        }

        return dp;
    }

    internal static DynamicParameters AddContestStagesInputParameter(this DynamicParameters dp, ContestStageFilter contestStage)
    {
        DataTable tvp = new();
        tvp.Columns.Add("value", typeof(int));

        switch (contestStage)
        {
            case ContestStageFilter.Any:
                tvp.Rows.Add((int)ContestStage.SemiFinal1);
                tvp.Rows.Add((int)ContestStage.SemiFinal2);
                tvp.Rows.Add((int)ContestStage.GrandFinal);

                break;
            case ContestStageFilter.SemiFinal1:
                tvp.Rows.Add((int)ContestStage.SemiFinal1);

                break;
            case ContestStageFilter.SemiFinal2:
                tvp.Rows.Add((int)ContestStage.SemiFinal2);

                break;
            case ContestStageFilter.SemiFinals:
                tvp.Rows.Add((int)ContestStage.SemiFinal1);
                tvp.Rows.Add((int)ContestStage.SemiFinal2);

                break;
            case ContestStageFilter.GrandFinal:
                tvp.Rows.Add((int)ContestStage.GrandFinal);

                break;
            default:
                throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStageFilter));
        }

        dp.Add("@contest_stages", tvp, DbType.Object, ParameterDirection.Input);

        return dp;
    }

    internal static DynamicParameters AddPaginationInputParameters(
        this DynamicParameters dp,
        int pageIndex = 0,
        int pageSize = 10,
        bool descending = false)
    {
        dp.Add("@page_index", pageIndex, DbType.Int32, ParameterDirection.Input);
        dp.Add("@page_size", pageSize, DbType.Int32, ParameterDirection.Input);
        dp.Add("@descending", descending, DbType.Boolean, ParameterDirection.Input);

        return dp;
    }

    internal static DynamicParameters AddPointsValueInputParameters(
        this DynamicParameters dp,
        int minPoints = 0,
        int maxPoints = 12)
    {
        dp.Add("@min_points_value", minPoints, DbType.Int32, ParameterDirection.Input);
        dp.Add("@max_points_value", maxPoints, DbType.Int32, ParameterDirection.Input);

        return dp;
    }
}
