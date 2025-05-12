using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.Documentation;

internal sealed class V0SchemaExampleTransformer : SchemaExampleTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(VotingMethod[])] = Enum.GetValues<VotingMethod>().ToOpenApiAny(),
            [typeof(PaginationMetadata)] = PaginationMetadata.CreateExample().ToOpenApiAny(),
            [typeof(GetPointsShareVotingCountryRankings.FilteringMetadata)] =
                GetPointsShareVotingCountryRankings.FilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(GetPointsShareVotingCountryRankings.Ranking)] =
                GetPointsShareVotingCountryRankings.Ranking.CreateExample().ToOpenApiAny()
        };
}
