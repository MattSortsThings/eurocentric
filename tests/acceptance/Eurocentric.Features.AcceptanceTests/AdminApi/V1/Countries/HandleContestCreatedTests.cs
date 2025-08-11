using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class HandleContestCreatedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task DomainEventHandler_should_update_all_participating_countries_when_Liverpool_format_contest_created(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_contest_for_my_countries(contestFormat: "Liverpool",
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_following_countries_should_now_participate_in_the_created_contest_only(
            "AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Then_the_following_countries_should_still_have_no_participating_contests("GB", "HR", "IT");
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task DomainEventHandler_should_update_all_participating_countries_when_Stockholm_format_contest_created(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_contest_for_my_countries(contestFormat: "Stockholm",
            group1CountryCodes: ["BE", "DK", "IT"],
            group2CountryCodes: ["CZ", "GB", "HR"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_following_countries_should_now_participate_in_the_created_contest_only(
            "BE", "CZ", "DK", "GB", "HR", "IT");
        await admin.Then_the_following_countries_should_still_have_no_participating_contests("AT", "EE", "FI", "XX");
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<CreateContestResponse>(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            CountryIds.Populate(createdCountries);
        }

        public void Given_I_want_to_create_a_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string? group0CountryCode = null,
            string contestFormat = "")
        {
            CreateContestRequest requestBody = new()
            {
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                CityName = TestDefaults.CityName,
                ContestYear = TestDefaults.ContestYear,
                Group0ParticipatingCountryId = group0CountryCode is not null ? CountryIds.GetSingle(group0CountryCode) : null,
                Group1ParticipantData = CountryIds.GetMultiple(group1CountryCodes)
                    .Select(TestDefaults.ParticipantDatum)
                    .ToArray(),
                Group2ParticipantData = CountryIds.GetMultiple(group2CountryCodes)
                    .Select(TestDefaults.ParticipantDatum)
                    .ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public async Task Then_the_following_countries_should_now_participate_in_the_created_contest_only(
            params string[] countryCodes)
        {
            CreateContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Guid createdContestId = responseBody.Contest.Id;

            HashSet<Guid> expectedCountryIds = CountryIds.GetMultiple(countryCodes)
                .OrderBy(id => id)
                .ToHashSet();

            Country[] allCountries = await ApiDriver.GetAllCountriesAsync();

            Country[] actualCountries = allCountries.Where(country => country.ParticipatingContestIds.Length != 0)
                .ToArray();

            await Assert.That(actualCountries)
                .HasCount(expectedCountryIds.Count)
                .And.ContainsOnly(country => country.ParticipatingContestIds.Single() == createdContestId);
        }

        public async Task Then_the_following_countries_should_still_have_no_participating_contests(params string[] countryCodes)
        {
            Country[] allCountries = await ApiDriver.GetAllCountriesAsync();

            Guid[] expectedCountryIds = CountryIds.GetMultiple(countryCodes)
                .OrderBy(id => id)
                .ToArray();

            Guid[] actualCountryIds = allCountries
                .Where(country => country.ParticipatingContestIds.Length == 0)
                .Select(country => country.Id)
                .OrderBy(id => id)
                .ToArray();

            await Assert.That(actualCountryIds).IsEquivalentTo(expectedCountryIds);
        }
    }
}
