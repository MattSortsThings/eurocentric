using Eurocentric.Apis.Public.V1.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public static class NullableStringExtensions
{
    public static ContestStageFilter? ToNullableContestStageFilter(this string? nullableString) =>
        nullableString is not null ? Enum.Parse<ContestStageFilter>(nullableString) : null;

    public static VotingMethodFilter? ToNullableVotingMethodFilter(this string? nullableString) =>
        nullableString is not null ? Enum.Parse<VotingMethodFilter>(nullableString) : null;
}
