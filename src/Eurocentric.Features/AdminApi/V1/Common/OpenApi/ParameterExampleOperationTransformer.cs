using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class ParameterExampleOperationTransformer : BaseParameterExampleOperationTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["contestId"] = ExampleIds.Contests.Basel2025.ToOpenApiAny(),
            ["countryId"] = ExampleIds.Countries.Austria.ToOpenApiAny()
        };
}
