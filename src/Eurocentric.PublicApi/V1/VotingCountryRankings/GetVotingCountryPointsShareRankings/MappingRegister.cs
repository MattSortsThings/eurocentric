using Eurocentric.Domain.Queries.VotingCountryRankings;
using Mapster;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config) =>
        config.NewConfig<VotingCountryPointsSharePage, GetVotingCountryPointsShareRankingsResponse>();
}
