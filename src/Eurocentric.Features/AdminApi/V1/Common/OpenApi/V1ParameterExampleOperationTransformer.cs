using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class V1ParameterExampleOperationTransformer : ParameterExampleOperationTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["contestId"] = ExampleValues.ContestId.ToOpenApiAny(), ["countryId"] = ExampleValues.CountryId.ToOpenApiAny()
        };
}
