using System.ComponentModel;
using System.Data;
using Dapper;
using ErrorOr;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Errors;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.DataAccess;

internal sealed class StoredProcedureParams : DynamicParameters
{
    private readonly List<Error> _errors = [];

    private StoredProcedureParams() { }

    public PaginationMetadata GetPaginationMetadata()
    {
        int pageIndex = Get<int>("@page_index");
        int pageSize = Get<int>("@page_size");
        bool descending = Get<bool>("@descending");
        int totalItems = Get<int>("@total_items");

        return new PaginationMetadata
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Descending = descending,
            TotalItems = totalItems,
            TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public static ErrorOr<StoredProcedureParams> From<T>(T query) where T : class
    {
        StoredProcedureParams instance = new();

        if (query is IContestStageFilteringQuery csf)
        {
            instance.AddContestStageFiltering(csf);
        }

        if (query is IPaginatedQuery p)
        {
            instance.AddPagination(p);
        }

        if (query is IVotingCountryFilteringQuery vc)
        {
            instance.AddVotingCountryFiltering(vc);
        }

        if (query is IVotingMethodFilteringQuery vm)
        {
            instance.AddVotingMethodFiltering(vm);
        }

        if (query is IYearRangeFilteringQuery yr)
        {
            instance.AddYearRangeFiltering(yr);
        }

        return instance.FailIfErrors();
    }

    private void AddContestStageFiltering(IContestStageFilteringQuery query)
    {
        DataTable tvp = MapToEnumIntValueDataTable(query.ContestStage);

        Add("@contest_stages", tvp, DbType.Object, ParameterDirection.Input);
    }

    private void AddPagination(IPaginatedQuery query)
    {
        if (query.PageIndex < 0)
        {
            _errors.Add(QueryParamErrors.PageIndexOutOfRange(query.PageIndex));
        }
        else
        {
            Add("@page_index", query.PageIndex, DbType.Int32, ParameterDirection.Input);
        }

        if (query.PageSize < 1)
        {
            _errors.Add(QueryParamErrors.PageSizeOutOfRange(query.PageSize));
        }
        else
        {
            Add("@page_size", query.PageSize, DbType.Int32, ParameterDirection.Input);
        }

        Add("@descending", query.Descending, DbType.Boolean, ParameterDirection.Input);
        Add("@total_items", null, DbType.Int32, ParameterDirection.Output);
    }

    private void AddVotingCountryFiltering(IVotingCountryFilteringQuery query)
    {
        if (query.VotingCountryCode is not { } s)
        {
            return;
        }

        if (ValidCountryCodeValue(s))
        {
            Add("@voting_country_code", s, DbType.String, ParameterDirection.Input);
        }
        else
        {
            _errors.Add(QueryParamErrors.InvalidVotingCountryCode(s));
        }
    }

    private void AddVotingMethodFiltering(IVotingMethodFilteringQuery query)
    {
        (bool excludeJury, bool excludeTelevote) = MapToExcludeVotingMethodParams(query.VotingMethod);

        Add("@exclude_jury", excludeJury, DbType.Boolean, ParameterDirection.Input);
        Add("@exclude_televote", excludeTelevote, DbType.Boolean, ParameterDirection.Input);
    }

    private void AddYearRangeFiltering(IYearRangeFilteringQuery query)
    {
        (int? min, int? max) = (query.MinYear, query.MaxYear);

        if (min.HasValue && max.HasValue && min.Value > max.Value)
        {
            _errors.Add(QueryParamErrors.InvalidContestYearRange(min.Value, max.Value));
        }

        if (min.HasValue)
        {
            Add("@min_year", min.Value, DbType.Int32, ParameterDirection.Input);
        }

        if (max.HasValue)
        {
            Add("@max_year", max.Value, DbType.Int32, ParameterDirection.Input);
        }
    }

    private ErrorOr<StoredProcedureParams> FailIfErrors() => _errors.Count != 0 ? _errors : this;

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

    private static bool ValidCountryCodeValue(string value) => value.Length == 2 && value.All(char.IsAsciiLetterUpper);
}
