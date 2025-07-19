using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(Broadcast)] = Broadcast.CreateExample().ToOpenApiAny(),
            [typeof(CreateChildBroadcastRequest)] = CreateChildBroadcastRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateChildBroadcastResponse)] = CreateChildBroadcastResponse.CreateExample().ToOpenApiAny(),
            [typeof(CreateContestRequest)] = CreateContestRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateContestResponse)] = CreateContestResponse.CreateExample().ToOpenApiAny(),
            [typeof(CreateCountryRequest)] = CreateCountryRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateCountryResponse)] = CreateCountryResponse.CreateExample().ToOpenApiAny(),
            [typeof(Contest)] = Contest.CreateExample().ToOpenApiAny(),
            [typeof(Country)] = Country.CreateExample().ToOpenApiAny()
        };
}
