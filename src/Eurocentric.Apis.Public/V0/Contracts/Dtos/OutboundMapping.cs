namespace Eurocentric.Apis.Public.V0.Contracts.Dtos;

internal static class OutboundMapping
{
    internal static ContestStage ToApiContestStage(this Domain.Enums.ContestStage stage)
    {
        return stage switch
        {
            Domain.Enums.ContestStage.SemiFinal1 => ContestStage.SemiFinal1,
            Domain.Enums.ContestStage.SemiFinal2 => ContestStage.SemiFinal2,
            Domain.Enums.ContestStage.GrandFinal => ContestStage.GrandFinal,
            _ => throw new ArgumentException($"ContestStage value {stage} not supported."),
        };
    }
}
