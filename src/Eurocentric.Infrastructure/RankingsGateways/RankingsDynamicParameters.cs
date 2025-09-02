using System.ComponentModel;
using System.Data;
using Dapper;
using ErrorOr;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Infrastructure.RankingsGateways;

internal sealed class RankingsDynamicParameters : DynamicParameters
{
    private RankingsDynamicParameters() { }

    internal PaginationOutputs GetPaginationOutputs()
    {
        (int totalItems, int pageSize) = (Get<int>("@total_items"), Get<int>("@page_size"));
        int totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PaginationOutputs(totalItems, totalPages);
    }

    internal sealed class Builder
    {
        private readonly RankingsDynamicParameters _parameters;

        private Builder(RankingsDynamicParameters parameters)
        {
            _parameters = parameters;
        }

        internal Builder AddPointsValueRange(int minPoints = 1, int maxPoints = 12)
        {
            _parameters.Add("@min_points", minPoints, DbType.Int32, ParameterDirection.Input);
            _parameters.Add("@max_points", maxPoints, DbType.Int32, ParameterDirection.Input);

            return this;
        }

        internal Builder AddOptionalContestYearRange(int? minYear = null, int? maxYear = null)
        {
            if (minYear is { } min)
            {
                _parameters.Add("@min_year", min, DbType.Int32, ParameterDirection.Input);
            }

            if (maxYear is { } max)
            {
                _parameters.Add("@max_year", max, DbType.Int32, ParameterDirection.Input);
            }

            return this;
        }

        internal Builder AddOptionalContestStages(ContestStageFilter? contestStage)
        {
            DataTable tvp = MapToEnumIntValueDataTable(contestStage.GetValueOrDefault(ContestStageFilter.Any));

            _parameters.Add("@contest_stages", tvp, DbType.Object, ParameterDirection.Input);

            return this;
        }

        internal Builder AddOptionalVotingCountry(string? votingCountryCode = null)
        {
            if (!string.IsNullOrWhiteSpace(votingCountryCode))
            {
                _parameters.Add("@voting_country_code", votingCountryCode, DbType.String, ParameterDirection.Input);
            }

            return this;
        }

        internal Builder AddOptionalVotingMethods(VotingMethodFilter? votingMethod)
        {
            switch (votingMethod)
            {
                case VotingMethodFilter.Jury:
                    _parameters.Add("@exclude_televote", true, DbType.Boolean, ParameterDirection.Input);

                    break;
                case VotingMethodFilter.Televote:
                    _parameters.Add("@exclude_jury", true, DbType.Boolean, ParameterDirection.Input);

                    break;
                case VotingMethodFilter.Any:
                case null:
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(VotingMethodFilter));
            }

            return this;
        }


        internal ErrorOr<RankingsDynamicParameters> Get()
        {
            (int pageIndex, int pageSize) = (_parameters.Get<int>("@page_index"), _parameters.Get<int>("@page_size"));

            if (pageIndex < 0 || pageSize < 1)
            {
                return IllegalPaginationValues(pageIndex, pageSize);
            }

            return _parameters;
        }

        internal static Builder InitializeWithPagination(int pageIndex = 0, int pageSize = 10, bool descending = false)
        {
            RankingsDynamicParameters parameters = new();
            parameters.Add("@total_items", null, DbType.Int32, ParameterDirection.Output);
            parameters.Add("@page_index", pageIndex, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@page_size", pageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@descending", descending, DbType.Boolean, ParameterDirection.Input);

            return new Builder(parameters);
        }


        private static Error IllegalPaginationValues(int pageIndex, int pageSize) => Error.Failure("Illegal pagination values",
            "Pagination page index must be non-negative and page size must be greater than zero.",
            new Dictionary<string, object> { { nameof(pageIndex), pageIndex }, { nameof(pageSize), pageSize } });

        private static DataTable MapToEnumIntValueDataTable(ContestStageFilter contestStage)
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

            return tvp;
        }
    }
}
