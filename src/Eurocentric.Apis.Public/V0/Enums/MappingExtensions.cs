using System.ComponentModel;
using ApiContestStage = Eurocentric.Apis.Public.V0.Enums.ContestStage;
using ApiContestStageFilter = Eurocentric.Apis.Public.V0.Enums.ContestStageFilter;
using ApiVotingMethodFilter = Eurocentric.Apis.Public.V0.Enums.VotingMethodFilter;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using DomainContestStageFilter = Eurocentric.Domain.Enums.ContestStageFilter;
using DomainVotingMethodFilter = Eurocentric.Domain.Enums.VotingMethodFilter;

namespace Eurocentric.Apis.Public.V0.Enums;

internal static class MappingExtensions
{
    internal static ApiContestStage ToApiContestStage(this DomainContestStage value) => (ApiContestStage)(int)value;

    internal static ApiContestStageFilter ToApiContestStageFilter(this DomainContestStageFilter value) =>
        (ApiContestStageFilter)(int)value;

    internal static ApiVotingMethodFilter ToApiVotingMethodFilter(this DomainVotingMethodFilter value) =>
        (ApiVotingMethodFilter)(int)value;

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

    internal static DomainContestStageFilter ToDomainContestStageFilter(this ApiContestStageFilter value)
    {
        return value switch
        {
            ApiContestStageFilter.Any => DomainContestStageFilter.Any,
            ApiContestStageFilter.SemiFinal1 => DomainContestStageFilter.SemiFinal1,
            ApiContestStageFilter.SemiFinal2 => DomainContestStageFilter.SemiFinal2,
            ApiContestStageFilter.SemiFinals => DomainContestStageFilter.SemiFinals,
            ApiContestStageFilter.GrandFinal => DomainContestStageFilter.GrandFinal,
            _ => throw new InvalidEnumArgumentException($"Invalid ContestStageFilter enum value: {value}."),
        };
    }

    internal static DomainVotingMethodFilter ToDomainVotingMethodFilter(this ApiVotingMethodFilter value)
    {
        return value switch
        {
            ApiVotingMethodFilter.Any => DomainVotingMethodFilter.Any,
            ApiVotingMethodFilter.Jury => DomainVotingMethodFilter.Jury,
            ApiVotingMethodFilter.Televote => DomainVotingMethodFilter.Televote,
            _ => throw new InvalidEnumArgumentException($"Invalid VotingMethodFilter enum value: {value}."),
        };
    }
}
