using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableBroadcasts;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContests;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableCountries;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsAverageRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsShareRankings;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V1.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(CompetingCountryPointsAverageFilteringMetadata)] =
                CompetingCountryPointsAverageFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsAverageRanking)] =
                CompetingCountryPointsAverageRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeFilteringMetadata)] =
                CompetingCountryPointsInRangeFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeRanking)] =
                CompetingCountryPointsInRangeRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsShareFilteringMetadata)] =
                CompetingCountryPointsShareFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsShareRanking)] =
                CompetingCountryPointsShareRanking.CreateExample().ToOpenApiAny(),
            [typeof(PaginationMetadata)] = PaginationMetadata.CreateExample().ToOpenApiAny(),
            [typeof(QueryableBroadcast)] = QueryableBroadcast.CreateExample().ToOpenApiAny(),
            [typeof(QueryableContestStage[])] = Enum.GetValues<QueryableContestStage>().ToOpenApiAny(),
            [typeof(QueryableContest)] = QueryableContest.CreateExample().ToOpenApiAny(),
            [typeof(QueryableCountry)] = QueryableCountry.CreateExample().ToOpenApiAny(),
            [typeof(QueryableVotingMethod[])] = Enum.GetValues<QueryableVotingMethod>().ToOpenApiAny()
        };
}
