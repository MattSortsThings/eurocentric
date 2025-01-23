using Mapster;
using ApiVotingMethod = Eurocentric.PublicApi.V0.Models.VotingMethod;
using DomainVotingMethod = Eurocentric.Domain.Queries.Common.VotingMethod;

namespace Eurocentric.PublicApi.V0.Models;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config) => config.NewConfig<ApiVotingMethod, DomainVotingMethod>().TwoWays();
}
