using Mapster;
using DomainContest = Eurocentric.Domain.Entities.Contests.Contest;

namespace Eurocentric.AdminApi.V0.Contests.CreateContest;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateContestRequest, CreateContestCommand>();
        config.NewConfig<DomainContest, CreateContestResponse>().Map(response => response.Contest, contest => contest);
    }
}
