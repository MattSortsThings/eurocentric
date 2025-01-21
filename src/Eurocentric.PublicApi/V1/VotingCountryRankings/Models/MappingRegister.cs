using Mapster;
using ApiVotingCountryPointsShareItem = Eurocentric.PublicApi.V1.VotingCountryRankings.Models.VotingCountryPointsShareItem;
using ApiVotingCountryPointsShareMeta = Eurocentric.PublicApi.V1.VotingCountryRankings.Models.VotingCountryPointsShareMetadata;
using DomainVotingCountryPointsShareItem = Eurocentric.Domain.Queries.VotingCountryRankings.VotingCountryPointsShareItem;
using DomainVotingCountryPointsShareMeta = Eurocentric.Domain.Queries.VotingCountryRankings.VotingCountryPointsShareMetadata;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.Models;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DomainVotingCountryPointsShareItem, ApiVotingCountryPointsShareItem>();
        config.NewConfig<DomainVotingCountryPointsShareMeta, ApiVotingCountryPointsShareMeta>();
    }
}
