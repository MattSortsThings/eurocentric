using System.ComponentModel;
using System.Data;
using ErrorOr;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Errors;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.DataAccess;

internal static class StoredProcedureParamsExtensions
{
    private const int FixedCountryCodeLength = 2;

    internal static ErrorOr<StoredProcedureParams> WithContestStagesParamFrom(this ErrorOr<StoredProcedureParams> p,
        IContestStageFilteringQuery query)
    {
        if (p.IsError)
        {
            return p;
        }

        StoredProcedureParams pp = p.Value;

        DataTable tvp = MapToEnumIntValueDataTable(query.ContestStage);

        pp.Add("@contest_stages", tvp, DbType.Object, ParameterDirection.Input);

        return pp;
    }

    internal static ErrorOr<StoredProcedureParams> WithPointsRangeParamsFrom(this ErrorOr<StoredProcedureParams> p,
        IPointsRangeFilteringQuery query)
    {
        if (p.IsError)
        {
            return p;
        }

        StoredProcedureParams pp = p.Value;

        (int minPoints, int maxPoints) = (query.MinPoints, query.MaxPoints);

        pp.Add("@min_points", minPoints, DbType.Int32, ParameterDirection.Input);
        pp.Add("@max_points", maxPoints, DbType.Int32, ParameterDirection.Input);

        return pp;
    }

    internal static ErrorOr<StoredProcedureParams> WithOptionalVotingCountryCodeParamFrom(this ErrorOr<StoredProcedureParams> p,
        IOptionalVotingCountryFilteringQuery query)
    {
        if (p.IsError || query.VotingCountryCode is not { } countryCode)
        {
            return p;
        }

        if (!ValidCountryCodeValue(countryCode))
        {
            return QueryParamErrors.InvalidVotingCountryCode(countryCode);
        }

        StoredProcedureParams pp = p.Value;

        pp.Add("@voting_country_code",
            countryCode,
            DbType.StringFixedLength,
            size: FixedCountryCodeLength,
            direction: ParameterDirection.Input);

        return pp;
    }

    internal static ErrorOr<StoredProcedureParams> WithVotingMethodParamsFrom(this ErrorOr<StoredProcedureParams> p,
        IVotingMethodFilteringQuery query)
    {
        if (p.IsError)
        {
            return p;
        }

        StoredProcedureParams pp = p.Value;

        (bool excludeJury, bool excludeTelevote) = MapToExcludeVotingMethodParams(query.VotingMethod);

        pp.Add("@exclude_jury", excludeJury, DbType.Boolean, ParameterDirection.Input);
        pp.Add("@exclude_televote", excludeTelevote, DbType.Boolean, ParameterDirection.Input);

        return pp;
    }

    internal static ErrorOr<StoredProcedureParams> WithOptionalYearRangeParamsFrom(this ErrorOr<StoredProcedureParams> p,
        IOptionalYearRangeFilteringQuery query)
    {
        if (p.IsError)
        {
            return p;
        }

        StoredProcedureParams pp = p.Value;

        if (query.MinYear is { } min)
        {
            pp.Add("@min_year", min, DbType.Int32, ParameterDirection.Input);
        }

        if (query.MaxYear is { } max)
        {
            pp.Add("@max_year", max, DbType.Int32, ParameterDirection.Input);
        }

        return pp;
    }

    private static DataTable MapToEnumIntValueDataTable(QueryableContestStage contestStage)
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

        return tvp;
    }

    private static (bool ExcludeJury, bool ExcludeTelevote) MapToExcludeVotingMethodParams(QueryableVotingMethod votingMethod) =>
        votingMethod switch
        {
            QueryableVotingMethod.Any => (ExcludeJury: false, ExcludeTelevote: false),
            QueryableVotingMethod.Jury => (ExcludeJury: false, ExcludeTelevote: true),
            QueryableVotingMethod.Televote => (ExcludeJury: true, ExcludeTelevote: false),
            _ => throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(QueryableVotingMethod))
        };

    private static bool ValidCountryCodeValue(string value) =>
        value.Length == FixedCountryCodeLength && value.All(char.IsAsciiLetterUpper);
}
