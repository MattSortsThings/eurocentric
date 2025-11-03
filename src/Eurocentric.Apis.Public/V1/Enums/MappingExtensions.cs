using System.ComponentModel;
using ApiContestStage = Eurocentric.Apis.Public.V1.Enums.ContestStage;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;

namespace Eurocentric.Apis.Public.V1.Enums;

internal static class MappingExtensions
{
    internal static ApiContestStage ToApiContestStage(this DomainContestStage value) => (ApiContestStage)(int)value;

    internal static DomainContestStage ToDomainContestStage(this ApiContestStage value)
    {
        return value switch
        {
            ApiContestStage.SemiFinal1 => DomainContestStage.SemiFinal1,
            ApiContestStage.SemiFinal2 => DomainContestStage.SemiFinal2,
            ApiContestStage.GrandFinal => DomainContestStage.GrandFinal,
            _ => throw new InvalidEnumArgumentException($"Invalid ContestStage enum value: {value}."),
        };
    }
}
