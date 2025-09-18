using ErrorOr;

namespace Eurocentric.Domain.V0Analytics.Scoreboard;

public interface IScoreboardGateway
{
    Task<ErrorOr<Scoreboard>> GetScoreboardAsync(ScoreboardQuery query, CancellationToken cancellationToken = default);
}
