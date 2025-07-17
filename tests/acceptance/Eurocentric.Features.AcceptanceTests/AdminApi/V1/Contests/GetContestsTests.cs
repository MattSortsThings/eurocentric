using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static class GetContestsTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_all_existing_contests_in_contest_year_order(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2024,
                cityName: "Malmö",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2016,
                cityName: "Stockholm",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2023,
                cityName: "Liverpool",
                group0CountryCode: "XX",
                group1CountryCodes: ["DK", "EE", "FI"],
                group2CountryCodes: ["AT", "BE", "CZ"]);
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2022,
                cityName: "Turin",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_retrieve_all_existing_contests();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_empty_list_when_no_contests_exist(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            admin.Given_I_want_to_retrieve_all_existing_contests();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contests_should_be_an_empty_list();
        }
    }

    private sealed class Admin : AdminActorWithResponse<GetContestsResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_retrieve_all_existing_contests() => Request = RequestFactory.Contests.GetContests();

        public void Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Contest> expectedContests = GivenContests.GetAll()
                .OrderBy(contest => contest.ContestYear);

            Contest[] actualContests = ResponseObject.Contests;

            Assert.Equal(expectedContests, actualContests, new ContestEqualityComparer());
        }

        public void Then_the_retrieved_contests_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Contests);
        }
    }
}
