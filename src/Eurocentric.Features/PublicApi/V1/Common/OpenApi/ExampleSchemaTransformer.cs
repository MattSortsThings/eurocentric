using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableBroadcasts;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContests;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableCountries;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsConsensusRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsShareRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsAverageRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsConsensusRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsShareRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V1.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(CompetingCountryPointsAverageQueryMetadata)] =
                CompetingCountryPointsAverageQueryMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsAverageRanking)] =
                CompetingCountryPointsAverageRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsConsensusQueryMetadata)] =
                CompetingCountryPointsConsensusQueryMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsConsensusRanking)] =
                CompetingCountryPointsConsensusRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeQueryMetadata)] =
                CompetingCountryPointsInRangeQueryMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeRanking)] =
                CompetingCountryPointsInRangeRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsShareFilteringMetadata)] =
                CompetingCountryPointsShareFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsShareRanking)] =
                CompetingCountryPointsShareRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsAverageFilteringMetadata)] =
                CompetitorPointsAverageFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsAverageRanking)] = CompetitorPointsAverageRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsConsensusFilteringMetadata)] =
                CompetitorPointsConsensusFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsConsensusRanking)] = CompetitorPointsConsensusRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsInRangeFilteringMetadata)] =
                CompetitorPointsInRangeFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsInRangeRanking)] = CompetitorPointsInRangeRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsShareFilteringMetadata)] =
                CompetitorPointsShareFilteringMetadata.CreateExample().ToOpenApiAny(),
            [typeof(CompetitorPointsShareRanking)] = CompetitorPointsShareRanking.CreateExample().ToOpenApiAny(),
            [typeof(PaginationMetadata)] = PaginationMetadata.CreateExample().ToOpenApiAny(),
            [typeof(QueryableBroadcast)] = QueryableBroadcast.CreateExample().ToOpenApiAny(),
            [typeof(QueryableContestStage[])] = Enum.GetValues<QueryableContestStage>().ToOpenApiAny(),
            [typeof(QueryableContest)] = QueryableContest.CreateExample().ToOpenApiAny(),
            [typeof(QueryableCountry)] = QueryableCountry.CreateExample().ToOpenApiAny(),
            [typeof(QueryableVotingMethod[])] = Enum.GetValues<QueryableVotingMethod>().ToOpenApiAny(),
            [typeof(VotingCountryPointsAverageQueryMetadata)] =
                VotingCountryPointsAverageQueryMetadata.CreateExample().ToOpenApiAny(),
            [typeof(VotingCountryPointsAverageRanking)] = VotingCountryPointsAverageRanking.CreateExample().ToOpenApiAny()
        };
}
