using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.Documentation;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(VotingMethod[])] = Enum.GetValues<VotingMethod>().ToOpenApiAny(),
            [typeof(PaginationMetadata)] = PaginationMetadata.CreateExample().ToOpenApiAny(),
            [typeof(PointsShareVotingCountryFilteringMetadata)] =
                PointsShareVotingCountryFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(PointsShareVotingCountryRanking)] =
                PointsShareVotingCountryRanking.CreateExample().ToOpenApiAny()
        };
}
