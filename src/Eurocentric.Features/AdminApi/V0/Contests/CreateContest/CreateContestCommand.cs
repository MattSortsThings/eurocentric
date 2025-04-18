using Eurocentric.Features.AdminApi.V0.Contests.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

public sealed record CreateContestCommand : ICommand<CreateContestResponse>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat Format { get; init; }
}
