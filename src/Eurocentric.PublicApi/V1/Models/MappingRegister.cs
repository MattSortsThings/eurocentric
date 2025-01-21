using Mapster;
using ApiVotingMethod = Eurocentric.PublicApi.V1.Models.VotingMethod;
using DomainVotingMethod = Eurocentric.Domain.Queries.Common.VotingMethod;

namespace Eurocentric.PublicApi.V1.Models;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config) => config.NewConfig<ApiVotingMethod, DomainVotingMethod>().TwoWays();
}
