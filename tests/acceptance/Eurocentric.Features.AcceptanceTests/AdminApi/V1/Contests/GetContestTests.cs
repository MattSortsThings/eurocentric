using System.Net;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DomainContest = Eurocentric.Domain.Contests.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_contest(group1Countries: ["AT", "BE", "CZ"],
            group2Countries: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_retrieve_non_existent_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_contest(group1Countries: ["AT", "BE", "CZ"],
            group2Countries: ["DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_contest();
        admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.NotFound);
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key("contestId");
    }

    private sealed class AdminActor : ActorWithResponse<GetContestResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(10);

        private Guid MyContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest(string[]? group2Countries = null, string[]? group1Countries = null)
        {
            CountryId[] group1CountryIds = group1Countries?.Select(code => MyCountryCodesAndIds[code])
                .Select(CountryId.FromValue)
                .ToArray() ?? [];

            CountryId[] group2CountryIds = group2Countries?.Select(code => MyCountryCodesAndIds[code])
                .Select(CountryId.FromValue)
                .ToArray() ?? [];

            DomainContest contest = StockholmFormatContest.Create(ContestYear.FromValue(2022).Value,
                CityName.FromValue("Turin").Value,
                group1CountryIds,
                group2CountryIds);

            Func<IServiceProvider, Task> add = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Contests.Add(contest);
                await dbContext.SaveChangesAsync();
            };

            await BackDoor.ExecuteScopedAsync(add);

            MyContestId = contest.Id.Value;
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            ContestId targetId = ContestId.FromValue(MyContestId);

            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                await dbContext.Contests.Where(contest => contest.Id == targetId)
                    .ExecuteDeleteAsync();
            };

            await BackDoor.ExecuteScopedAsync(delete);
        }

        public void Given_I_want_to_retrieve_my_contest_by_its_ID() =>
            SendMyRequest = apiDriver => apiDriver.Contests.GetContest(MyContestId, TestContext.Current.CancellationToken);

        public void Then_the_retrieved_contest_should_be_my_contest()
        {
            Assert.NotNull(ResponseObject);

            Assert.Equal(MyContestId, ResponseObject.Contest.Id);
        }

        public void Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key(string key) =>
            Then_the_problem_details_extensions_should_contain(key, MyContestId);
    }
}
