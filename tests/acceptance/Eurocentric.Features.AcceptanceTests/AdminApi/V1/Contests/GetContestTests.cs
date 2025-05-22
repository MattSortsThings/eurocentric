using System.Net;
using System.Text.Json;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.Extensions.DependencyInjection;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests : AcceptanceTestBase
{
    public GetContestTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_retrieve_a_contest_by_its_ID()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        admin.Given_I_want_to_retrieve_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Fact]
    public async Task Should_be_unable_to_retrieve_a_non_existent_contest_by_its_ID()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        admin.Given_I_want_to_retrieve_my_contest();
        await admin.Given_I_have_deleted_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.NotFound);
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_contest_ID();
    }

    private sealed class AdminActor : ActorWithResponse<GetContestResponse>
    {
        private readonly WebAppFixtureBackdoor _backdoor;
        private readonly AdminApiV1Driver _driver;

        private AdminActor(WebAppFixtureBackdoor backdoor, AdminApiV1Driver driver)
        {
            _backdoor = backdoor;
            _driver = driver;
        }

        private Dictionary<string, Guid> MyCountries { get; } = new(7);

        private Contest? MyContest { get; set; }

        public async Task Given_I_have_created_the_countries(params string[] countryCodes)
        {
            Country[] countries = await
                _driver.CreateMultipleCountriesAsync(countryCodes, TestContext.Current.CancellationToken);

            foreach (Country country in countries)
            {
                MyCountries.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest(
            string[]? group1Participants = null,
            string[]? group2Participants = null,
            string contestFormat = "Stockholm")
        {
            CreateContestRequest request = new()
            {
                ContestYear = 2022,
                CityName = "Turin",
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                Group1Participants = group1Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? [],
                Group2Participants = group2Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? []
            };

            ResponseOrProblem<CreateContestResponse> responseOrProblem =
                await _driver.CreateContestAsync(request, TestContext.Current.CancellationToken);

            MyContest = responseOrProblem.AsT0.Data!.Contest;
        }

        public void Then_the_problem_details_extensions_should_contain_my_contest_ID()
        {
            Assert.NotNull(ProblemDetails);

            Assert.Contains(ProblemDetails.Extensions,
                kvp => kvp is { Key: "contestId", Value: JsonElement j } && j.GetGuid() == MyContest!.Id);
        }

        public static AdminActor WithDriverAndBackdoor(AdminApiV1Driver driver, WebAppFixtureBackdoor backdoor) =>
            new(backdoor, driver);

        public async Task Given_I_have_deleted_my_contest() => await _backdoor.DeleteContestAsync(MyContest!.Id);

        public void Given_I_want_to_retrieve_my_contest()
        {
            Assert.NotNull(MyContest);

            SendMyRequest = () => _driver.GetContestAsync(MyContest.Id, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_contest_should_be_my_contest()
        {
            Assert.NotNull(MyContest);
            Assert.NotNull(Response);

            Assert.Equal(MyContest, Response.Contest, new ContestEqualityComparer());
        }
    }

    private sealed class WebAppFixtureBackdoor(WebAppFixture fixture)
    {
        public async Task DeleteContestAsync(Guid contestId)
        {
            ContestId targetId = ContestId.FromValue(contestId);

            Func<IServiceProvider, Task> persist = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Contests.Remove(dbContext.Contests.First(c => c.Id == targetId));
                await dbContext.SaveChangesAsync();
            };

            await fixture.ExecuteScopedAsync(persist);
        }
    }
}
