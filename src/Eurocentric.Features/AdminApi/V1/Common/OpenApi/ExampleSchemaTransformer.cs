using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
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
            [typeof(AwardJuryPointsRequest)] = AwardJuryPointsRequest.CreateExample().ToOpenApiAny(),
            [typeof(AwardTelevotePointsRequest)] = AwardTelevotePointsRequest.CreateExample().ToOpenApiAny(),
            [typeof(Award)] = Award.CreateExample().ToOpenApiAny(),
            [typeof(Broadcast)] = Broadcast.CreateExample().ToOpenApiAny(),
            [typeof(BroadcastMemo)] = BroadcastMemo.CreateExample().ToOpenApiAny(),
            [typeof(Competitor)] = Competitor.CreateExample().ToOpenApiAny(),
            [typeof(Contest)] = Contest.CreateExample().ToOpenApiAny(),
            [typeof(ContestMemo)] = ContestMemo.CreateExample().ToOpenApiAny(),
            [typeof(Country)] = Country.CreateExample().ToOpenApiAny(),
            [typeof(CreateChildBroadcastRequest)] = CreateChildBroadcastRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateChildBroadcastResponse)] = CreateChildBroadcastResponse.CreateExample().ToOpenApiAny(),
            [typeof(CreateContestRequest)] = CreateContestRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateContestResponse)] = CreateContestResponse.CreateExample().ToOpenApiAny(),
            [typeof(CreateCountryRequest)] = CreateCountryRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateCountryResponse)] = CreateCountryResponse.CreateExample().ToOpenApiAny(),
            [typeof(Participant)] = Participant.CreateExample().ToOpenApiAny(),
            [typeof(Voter)] = Voter.CreateExample().ToOpenApiAny()
        };
}
