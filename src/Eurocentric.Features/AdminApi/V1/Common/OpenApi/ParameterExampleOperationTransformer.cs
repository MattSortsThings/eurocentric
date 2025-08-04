using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal class ParameterExampleOperationTransformer : BaseParameterExampleOperationTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["broadcastId"] = ExampleValues.BroadcastId.ToOpenApiAny(),
            ["contestId"] = ExampleValues.ContestId.ToOpenApiAny(),
            ["countryId"] = ExampleValues.CountryId.ToOpenApiAny()
        };
}
