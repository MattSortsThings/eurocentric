using Eurocentric.Domain.Entities.Contests;
using Mapster;

namespace Eurocentric.AdminApi.V1.Contests.CreateContest;

internal sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateContestRequest, CreateContestCommand>();
        config.NewConfig<Contest, CreateContestResponse>().Map(response => response.Contest, contest => contest);
    }
}
