using System.ComponentModel;
using ApiContestRoleType = Eurocentric.Apis.Admin.V1.Enums.ContestRoleType;
using ApiContestRules = Eurocentric.Apis.Admin.V1.Enums.ContestRules;
using ApiContestStage = Eurocentric.Apis.Admin.V1.Enums.ContestStage;
using ApiSemiFinalDraw = Eurocentric.Apis.Admin.V1.Enums.SemiFinalDraw;
using DomainContestRoleType = Eurocentric.Domain.Enums.ContestRoleType;
using DomainContestRules = Eurocentric.Domain.Enums.ContestRules;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using DomainSemiFinalDraw = Eurocentric.Domain.Enums.SemiFinalDraw;

namespace Eurocentric.Apis.Admin.V1.Enums;

internal static class MappingExtensions
{
    internal static ApiContestRoleType ToApiContestRoleType(this DomainContestRoleType value) =>
        (ApiContestRoleType)(int)value;

    internal static ApiContestRules ToApiContestRules(this DomainContestRules value) => (ApiContestRules)(int)value;

    internal static ApiContestStage ToApiContestStage(this DomainContestStage value) => (ApiContestStage)(int)value;

    internal static ApiSemiFinalDraw ToApiSemiFinalDraw(this DomainSemiFinalDraw value) => (ApiSemiFinalDraw)(int)value;

    internal static DomainContestRules ToDomainContestRules(this ApiContestRules value)
    {
        return value switch
        {
            ApiContestRules.Liverpool => DomainContestRules.Liverpool,
            ApiContestRules.Stockholm => DomainContestRules.Stockholm,
            _ => throw new InvalidEnumArgumentException($"Invalid ContestRules enum value: {value}."),
        };
    }

    internal static DomainSemiFinalDraw ToDomainSemiFinalDraw(this ApiSemiFinalDraw value)
    {
        return value switch
        {
            ApiSemiFinalDraw.SemiFinal1 => DomainSemiFinalDraw.SemiFinal1,
            ApiSemiFinalDraw.SemiFinal2 => DomainSemiFinalDraw.SemiFinal2,
            _ => throw new InvalidEnumArgumentException($"Invalid SemiFinalDraw enum value: {value}."),
        };
    }
}
