using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.Documentation;

internal sealed class V0ParameterExampleTransformer : ParameterExampleTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["competingCountryCode"] = new OpenApiString("GB"),
            ["votingMethod"] = new OpenApiString("Televote"),
            ["contestStages"] = new OpenApiString("GrandFinal"),
            ["minYear"] = new OpenApiInteger(2016),
            ["maxYear"] = new OpenApiInteger(2025),
            ["pageIndex"] = new OpenApiInteger(PaginationDefaults.PageIndex),
            ["pageSize"] = new OpenApiInteger(PaginationDefaults.PageSize),
            ["descending"] = new OpenApiBoolean(PaginationDefaults.Descending)
        };
}
