using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class HandleContestCreatedTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_add_participating_contest_memo_to_all_participating_countries_when_contest_created_scenario_1(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Stockholm",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        await admin.Then_the_following_countries_should_have_no_participating_contests("GB", "HR", "IT");
        await admin.Then_the_following_countries_should_have_my_contest_as_their_only_participating_contest(
            "AT", "BE", "CZ", "DK", "EE", "FI");
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_add_participating_contest_memo_to_all_participating_countries_when_contest_created_scenario_2(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "LU", "XX");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Liverpool",
            group0CountryCode: "XX",
            group1CountryCodes: ["BE", "DK", "FI"],
            group2CountryCodes: ["GB", "HR", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        await admin.Then_the_following_countries_should_have_no_participating_contests("AT", "CZ", "EE", "LU");
        await admin.Then_the_following_countries_should_have_my_contest_as_their_only_participating_contest(
            "BE", "DK", "FI", "GB", "HR", "IT", "XX");
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_not_update_any_countries_when_contest_creation_fails(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "LU", "XX");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Liverpool",
            group1CountryCodes: ["BE"],
            group2CountryCodes: ["GB"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        await admin.Then_all_countries_should_have_no_participating_contests();
    }

    private sealed class AdminActor : ActorWithResponse<CreateContestResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(10);

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public void Given_I_want_to_create_a_contest(
            string? group0CountryCode = null,
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            string contestFormat = "")
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = 2025,
                CityName = "CityName",
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                Group0CountryId = group0CountryCode is null ? null : MyCountryCodesAndIds[group0CountryCode],
                Group1Participants =
                    group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToParticipantSpecifications() ?? [],
                Group2Participants =
                    group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToParticipantSpecifications() ?? []
            };

            SendMyRequest = apiDriver => apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public async Task Then_the_following_countries_should_have_no_participating_contests(params string[] countryCodes)
        {
            Country[] allExistingCountries =
                await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            IEnumerable<Country> countriesToVerify = allExistingCountries
                .Join(countryCodes,
                    country => country.CountryCode,
                    code => code,
                    (country, _) => country);

            Assert.All(countriesToVerify, country => Assert.Empty(country.ParticipatingContests));
        }

        public async Task Then_the_following_countries_should_have_my_contest_as_their_only_participating_contest(
            params string[] countryCodes)
        {
            Assert.NotNull(ResponseObject);

            ContestMemo expectedMemo = new(ResponseObject.Contest.Id, ContestStatus.Initialized);

            Country[] allExistingCountries =
                await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            IEnumerable<Country> countriesToVerify = allExistingCountries
                .Join(countryCodes,
                    country => country.CountryCode,
                    code => code,
                    (country, _) => country);

            Assert.All(countriesToVerify, country =>
            {
                ContestMemo memo = Assert.Single(country.ParticipatingContests);
                Assert.Equal(expectedMemo, memo);
            });
        }

        public async Task Then_all_countries_should_have_no_participating_contests()
        {
            Country[] countriesToVerify = await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            Assert.All(countriesToVerify, country => Assert.Empty(country.ParticipatingContests));
        }
    }
}
