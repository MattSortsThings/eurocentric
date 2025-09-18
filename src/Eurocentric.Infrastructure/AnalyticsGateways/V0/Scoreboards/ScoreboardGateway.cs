using ErrorOr;
using Eurocentric.Domain.V0Analytics.Scoreboard;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Eurocentric.Infrastructure.DataAccess.Dapper;

namespace Eurocentric.Infrastructure.AnalyticsGateways.V0.Scoreboards;

internal sealed class ScoreboardGateway(IDbSprocRunner sprocRunner) : IScoreboardGateway
{
    public async Task<ErrorOr<Scoreboard>> GetScoreboardAsync(ScoreboardQuery query,
        CancellationToken cancellationToken = default) => await query.ToErrorOr()
        .ThenAsync(scoreboardQuery => ExecuteSprocAsync(scoreboardQuery, cancellationToken)
            .Then(rows => MapToScoreboard(rows, scoreboardQuery)));

    private async Task<ErrorOr<ScoreboardRow[]>> ExecuteSprocAsync(ScoreboardQuery query, CancellationToken cancellationToken)
    {
        ScoreboardDynamicParameters parameters = BuildDynamicParameters(query);

        ScoreboardRow[] rows = await sprocRunner.ExecuteAsync<ScoreboardRow>(V0Schema.Sprocs.GetScoreboardRows,
            parameters,
            cancellationToken);

        return rows.Length == 0
            ? Error.NotFound("Broadcast not found",
                "No queryable broadcast matches the query.",
                new Dictionary<string, object> { { "contestYear", query.ContestYear }, { "contestStage", query.ContestStage } })
            : rows;
    }

    private static ScoreboardDynamicParameters BuildDynamicParameters(ScoreboardQuery query)
    {
        ScoreboardDynamicParameters dynamicParameters = new();
        dynamicParameters.AddContestYearParameter(query.ContestYear);
        dynamicParameters.AddContestStageParameter(query.ContestStage);

        return dynamicParameters;
    }

    private static Scoreboard MapToScoreboard(ScoreboardRow[] rows, ScoreboardQuery query)
    {
        ScoreboardMetadata metadata = new()
        {
            ContestYear = query.ContestYear,
            ContestStage = query.ContestStage,
            TelevoteOnlyBroadcast = rows.All(row => row.JuryPoints is null)
        };

        return new Scoreboard(rows, metadata);
    }
}
