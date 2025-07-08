using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public static class CreateContestTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_create_and_retrieve_contest_scenario_1(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm", contestYear: 2022, cityName: "Turin");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_contest_should_match(contestFormat: "Stockholm", contestYear: 2022, cityName: "Turin");
            await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
        }

        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_create_and_retrieve_contest_scenario_2(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool", contestYear: 2025, cityName: "Basel");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_contest_should_match(contestFormat: "Liverpool", contestYear: 2025, cityName: "Basel");
            await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
        }
    }

    private sealed class Admin : AdminActor<CreateContestResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_create_a_contest(string cityName = "", int contestYear = 0, string contestFormat = "")
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = contestYear, CityName = cityName, ContestFormat = Enum.Parse<ContestFormat>(contestFormat)
            };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Then_the_created_contest_should_match(string cityName = "", int contestYear = 0, string contestFormat = "")
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;

            Assert.Equal(cityName, createdContest.CityName);
            Assert.Equal(contestYear, createdContest.ContestYear);
            Assert.Equal(Enum.Parse<ContestFormat>(contestFormat), createdContest.ContestFormat);
        }

        public async Task Then_the_created_contest_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;
            Contest retrievedContest = await GetContestAsync(createdContest.Id);

            Assert.Equal(createdContest, retrievedContest, ContestEquality.Compare);
        }

        private async Task<Contest> GetContestAsync(Guid contestId)
        {
            RestRequest restRequest = RequestFactory.Contests.GetContest(contestId);

            ProblemOrResponse<GetContestResponse> problemOrResponse =
                await RestClient.SendAsync<GetContestResponse>(restRequest, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Contest;
        }
    }
}
