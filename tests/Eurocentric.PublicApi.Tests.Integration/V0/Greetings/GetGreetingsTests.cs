using Eurocentric.PublicApi.Tests.Integration.TestUtils;
using Eurocentric.PublicApi.V0.Greetings.Common;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(TestCaseSerializer),
    typeof(GetGreetingsRequest),
    typeof(GetGreetingsResponse))]

namespace Eurocentric.PublicApi.Tests.Integration.V0.Greetings;

public static class GetGreetingsTests
{
    public sealed class Feature : IntegrationTest
    {
        public Feature(SeededWebAppFixture webAppFixture) : base(webAppFixture)
        {
        }

        [Theory]
        [ClassData(typeof(TestCases))]
        public async Task Should_return_200_with_response_given_valid_request(GetGreetingsRequest request,
            GetGreetingsResponse expectedResponse)
        {
            // Act
            Ok<GetGreetingsResponse> result = await GetGreetingsEndpoint.ExecuteAsync(request,
                Sender,
                TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status200OK, result.StatusCode),
                () => Assert.Equivalent(expectedResponse, result.Value)
            );
        }
    }

    private sealed class TestCases : TheoryData<GetGreetingsRequest, GetGreetingsResponse>
    {
        public TestCases()
        {
            Add(new GetGreetingsRequest { Quantity = 1, Language = Language.English },
                new GetGreetingsResponse([new Greeting("hi!", Language.English)]));

            Add(new GetGreetingsRequest { Quantity = 3, Language = Language.Dutch },
                new GetGreetingsResponse([
                    new Greeting("hoi!", Language.Dutch),
                    new Greeting("hoi!", Language.Dutch),
                    new Greeting("hoi!", Language.Dutch)
                ]));
        }
    }
}
