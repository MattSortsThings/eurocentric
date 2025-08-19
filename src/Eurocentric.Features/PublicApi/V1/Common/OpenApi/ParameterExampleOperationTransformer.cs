using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V1.Common.OpenApi;

internal sealed class ParameterExampleOperationTransformer : BaseParameterExampleOperationTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["competingCountryCode"] = ExampleValues.CompetingCountryCode.ToOpenApiAny(),
            ["contestStage"] = QueryParamDefaults.ContestStage.ToOpenApiAny(),
            ["descending"] = QueryParamDefaults.Descending.ToOpenApiAny(),
            ["minPoints"] = ExampleValues.MinPoints.ToOpenApiAny(),
            ["maxPoints"] = ExampleValues.MaxPoints.ToOpenApiAny(),
            ["minYear"] = ExampleValues.MinYear.ToOpenApiAny(),
            ["maxYear"] = ExampleValues.MaxYear.ToOpenApiAny(),
            ["pageIndex"] = QueryParamDefaults.PageIndex.ToOpenApiAny(),
            ["pageSize"] = QueryParamDefaults.PageSize.ToOpenApiAny(),
            ["votingCountryCode"] = ExampleValues.VotingCountryCode.ToOpenApiAny(),
            ["votingMethod"] = QueryParamDefaults.VotingMethod.ToOpenApiAny()
        };
}
