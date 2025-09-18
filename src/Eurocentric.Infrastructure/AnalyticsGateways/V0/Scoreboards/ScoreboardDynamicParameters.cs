using System.Data;
using Dapper;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Infrastructure.AnalyticsGateways.V0.Scoreboards;

internal sealed class ScoreboardDynamicParameters : DynamicParameters
{
    internal void AddContestYearParameter(int contestYear) =>
        Add("@contest_year", contestYear, DbType.Int32, ParameterDirection.Input);

    internal void AddContestStageParameter(ContestStage contestStage) =>
        Add("@contest_stage", (int)contestStage, DbType.Int32, ParameterDirection.Input);
}
