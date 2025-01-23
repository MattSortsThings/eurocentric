using Mapster;
using ApiVotingCountryPointsShareMeta = Eurocentric.PublicApi.V0.VotingCountryRankings.Models.VotingCountryPointsShareMetadata;
using DomainVotingCountryPointsShareMeta = Eurocentric.Domain.Queries.VotingCountryRankings.VotingCountryPointsShareMetadata;

namespace Eurocentric.PublicApi.V0.VotingCountryRankings.Models;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Domain.Queries.VotingCountryRankings.VotingCountryPointsShareRanking, VotingCountryPointsShareRanking>();
        config.NewConfig<DomainVotingCountryPointsShareMeta, ApiVotingCountryPointsShareMeta>();
    }
}
