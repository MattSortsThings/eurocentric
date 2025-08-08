using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;

public sealed record CreateChildBroadcastRequest : IExampleProvider<CreateChildBroadcastRequest>
{
    public required DateOnly BroadcastDate { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required Guid[] CompetingCountryIds { get; init; }

    public static CreateChildBroadcastRequest CreateExample() => new()
    {
        BroadcastDate = new DateOnly(2025, 5, 17),
        ContestStage = ContestStage.GrandFinal,
        CompetingCountryIds = [ExampleValues.CountryId1Of3]
    };
}
