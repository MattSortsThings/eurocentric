using System.Net;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.Extensions.DependencyInjection;
using DomainContest = Eurocentric.Domain.Contests.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_all_existing_contests_in_contest_year_order(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR");
        await admin.Given_I_have_created_a_contest(contestYear: 2025,
            group1Countries: ["AT", "BE", "CZ"], group2Countries: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_contest(contestYear: 2016,
            group1Countries: ["AT", "BE", "CZ"], group2Countries: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_contest(contestYear: 2022,
            group1Countries: ["AT", "BE", "CZ"], group2Countries: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_contest(contestYear: 2018,
            group1Countries: ["AT", "BE", "CZ"], group2Countries: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contests_years_should_be(2016, 2018, 2022, 2025);
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_empty_list_when_no_contests_exist(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

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
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(8);

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest(string[]? group2Countries = null,
            string[]? group1Countries = null,
            int contestYear = 0)
        {
            CountryId[] group1CountryIds = group1Countries?.Select(code => MyCountryCodesAndIds[code])
                .Select(CountryId.FromValue)
                .ToArray() ?? [];

            CountryId[] group2CountryIds = group2Countries?.Select(code => MyCountryCodesAndIds[code])
                .Select(CountryId.FromValue)
                .ToArray() ?? [];

            DomainContest contest = StockholmFormatContest.Create(ContestYear.FromValue(contestYear).Value,
                CityName.FromValue("CityName").Value,
                group1CountryIds,
                group2CountryIds);

            Func<IServiceProvider, Task> add = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Contests.Add(contest);
                await dbContext.SaveChangesAsync();
            };

            await BackDoor.ExecuteScopedAsync(add);
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            SendMyRequest = apiDriver => apiDriver.Contests.GetContests(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_contests_years_should_be(params int[] years)
        {
            Assert.NotNull(ResponseObject);

            Assert.Equal(years, ResponseObject.Contests.Select(contest => contest.ContestYear));
        }

        public void Then_the_retrieved_contests_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Contests);
        }
    }
}
