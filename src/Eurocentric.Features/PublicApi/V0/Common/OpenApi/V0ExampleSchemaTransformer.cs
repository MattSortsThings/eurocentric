using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class V0ExampleSchemaTransformer : ExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(CompetingCountryPointsInRangeRanking)] =
                CompetingCountryPointsInRangeRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeMetadata)] =
                CompetingCountryPointsInRangeMetadata.CreateExample().ToOpenApiAny(),
            [typeof(QueryableContestStage[])] = Enum.GetValues<QueryableContestStage>().ToOpenApiAny(),
            [typeof(QueryableCountry)] = QueryableCountry.CreateExample().ToOpenApiAny(),
            [typeof(QueryableVotingMethod[])] = Enum.GetValues<QueryableVotingMethod>().ToOpenApiAny()
        };
}
