using Mapster;
using ApiContest = Eurocentric.AdminApi.V1.Contests.Models.Contest;
using ApiVotingRules = Eurocentric.AdminApi.V1.Contests.Models.VotingRules;
using DomainContest = Eurocentric.Domain.Entities.Contests.Contest;
using DomainVotingRules = Eurocentric.Domain.Entities.Contests.VotingRules;

namespace Eurocentric.AdminApi.V1.Contests.Models;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DomainContest, ApiContest>();
        config.NewConfig<DomainVotingRules, ApiVotingRules>().TwoWays();
    }
}
