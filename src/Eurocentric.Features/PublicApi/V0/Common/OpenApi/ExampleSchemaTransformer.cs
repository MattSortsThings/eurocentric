using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(PaginationMetadata)] = PaginationMetadata.CreateExample().ToOpenApiAny(),
            [typeof(ContestStages[])] = Enum.GetNames<ContestStages>().ToOpenApiAny(),
            [typeof(VotingMethod[])] = Enum.GetNames<VotingMethod>().ToOpenApiAny(),
            [typeof(PointsShareVotingCountryRankingFilters)] =
                PointsShareVotingCountryRankingFilters.CreateExample().ToOpenApiAny(),
            [typeof(PointsShareVotingCountryRanking)] = PointsShareVotingCountryRanking.CreateExample().ToOpenApiAny()
        };
}
