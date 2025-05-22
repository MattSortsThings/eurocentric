using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestsTests : AcceptanceTestBase
{
    public GetContestsTests(WebAppFixture webAppFixture) : base(webAppFixture)
    {
    }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_retrieve_all_existing_contests_in_year_order()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_the_contest(contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            group1Participants: ["BE", "DE", "FI", "HR"],
            group2Participants: ["AT", "CZ", "ES", "GB"]);
        await admin.Given_I_have_created_the_contest(contestFormat: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "FI", "HR"]);
        await admin.Given_I_have_created_the_contest(contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["ES", "FI", "GB", "HR"]);
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contests_should_be_my_contests_in_year_order();
    }

    [Fact]
    public async Task Should_be_able_to_retrieve_an_empty_list_when_no_contests_exist()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contests_should_be_an_empty_list();
    }

    private sealed class AdminActor : ActorWithResponse<GetContestsResponse>
    {
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver)
        {
            _driver = driver;
        }

        private Dictionary<string, Guid> MyCountries { get; } = new(7);

        private List<Contest> MyContests { get; } = new(3);

        public async Task Given_I_have_created_the_countries(params string[] countryCodes)
        {
            Country[] countries = await
                _driver.CreateMultipleCountriesAsync(countryCodes, TestContext.Current.CancellationToken);

            foreach (Country country in countries)
            {
                MyCountries.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_the_contest(int? contestYear = null,
            string? cityName = null,
            string? contestFormat = null,
            string? group0Participant = null,
            string[]? group1Participants = null,
            string[]? group2Participants = null)
        {
            CreateContestRequest request = new()
            {
                ContestYear = contestYear.GetValueOrDefault(2025),
                CityName = cityName ?? "CityName",
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat ?? "Stockholm"),
                Group0CountryId = group0Participant is not null ? MyCountries[group0Participant] : null,
                Group1Participants = group1Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? [],
                Group2Participants = group2Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? []
            };

            ResponseOrProblem<CreateContestResponse> responseOrProblem =
                await _driver.CreateContestAsync(request, TestContext.Current.CancellationToken);

            MyContests.Add(responseOrProblem.AsT0.Data!.Contest);
        }

        public static AdminActor WithDriver(AdminApiV1Driver driver) => new(driver);

        public void Given_I_want_to_retrieve_all_existing_contests() => SendMyRequest = () => _driver.GetContestsAsync();

        public void Then_the_retrieved_contests_should_be_my_contests_in_year_order()
        {
            Assert.NotNull(Response);

            Assert.Equal(MyContests.OrderBy(c => c.Year), Response.Contests, new ContestEqualityComparer());
        }

        public void Then_the_retrieved_contests_should_be_an_empty_list()
        {
            Assert.NotNull(Response);

            Assert.Empty(Response.Contests);
        }
    }
}
