using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.Documentation;

internal sealed class ParameterExampleOperationTransformer : BaseParameterExampleOperationTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["broadcastId"] = new OpenApiString(ExampleValues.BroadcastId.ToString()),
            ["contestId"] = new OpenApiString(ExampleValues.ContestId.ToString()),
            ["countryId"] = new OpenApiString(ExampleValues.CountryId.ToString())
        };
}
