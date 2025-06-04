using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public sealed class CreateContestTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v0.2")]
    public async Task Should_be_able_to_create_contest(string apiVersion)
    {
        AdminActor admin = new(AdminApiV0Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool", contestYear: 2025, cityName: "Basel");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_contest_should_match(contestFormat: "Liverpool", contestYear: 2025, cityName: "Basel");
    }

    private sealed class AdminActor : ActorWithResponse<CreateContestResponse>
    {
        public AdminActor(IAdminApiV0Driver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_create_a_contest(int contestYear = 0, string cityName = "", string contestFormat = "")
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = contestYear, CityName = cityName, ContestFormat = Enum.Parse<ContestFormat>(contestFormat)
            };

            SendMyRequest = apiDriver => apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Then_the_created_contest_should_match(int contestYear = 0, string cityName = "", string contestFormat = "")
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;

            Assert.Equal(contestYear, createdContest.ContestYear);
            Assert.Equal(cityName, createdContest.CityName);
            Assert.Equal(Enum.Parse<ContestFormat>(contestFormat), createdContest.ContestFormat);
        }
    }
}
