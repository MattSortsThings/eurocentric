using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V0.Common.Documentation;

internal sealed class V0SchemaExampleTransformer : SchemaExampleTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(Contest)] = Contest.CreateExample().ToOpenApiAny(),
            [typeof(CreateContest.Request)] = CreateContest.Request.CreateExample().ToOpenApiAny()
        };
}
