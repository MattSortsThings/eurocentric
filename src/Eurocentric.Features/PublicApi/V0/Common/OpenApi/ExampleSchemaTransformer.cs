using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.Rankings;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(CompetingCountryPointsAverageFilters)] =
                CompetingCountryPointsAverageFilters.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsAverageRanking)] =
                CompetingCountryPointsAverageRanking.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeFilters)] =
                CompetingCountryPointsInRangeFilters.CreateExample().ToOpenApiAny(),
            [typeof(CompetingCountryPointsInRangeRanking)] =
                CompetingCountryPointsInRangeRanking.CreateExample().ToOpenApiAny(),
            [typeof(Country)] = Country.CreateExample().ToOpenApiAny(),
            [typeof(GetContestStagesResponse)] = GetContestStagesResponse.CreateExample().ToOpenApiAny(),
            [typeof(GetVotingMethodsResponse)] = GetVotingMethodsResponse.CreateExample().ToOpenApiAny(),
            [typeof(PaginationInfo)] = PaginationInfo.CreateExample().ToOpenApiAny()
        };
}
