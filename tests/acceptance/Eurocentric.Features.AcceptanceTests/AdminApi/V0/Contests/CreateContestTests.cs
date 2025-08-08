using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Contests.CreateContest;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public static class CreateContestTests
{
    public sealed class CreateContest : SerialCleanAcceptanceTest
    {
        [Test]
        [Arguments("v0.1")]
        [Arguments("v0.2")]
        public async Task Should_create_contest_scenario_1(string apiVersion)
        {
            AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_contest_for_my_countries(contestFormat: "Liverpool",
                contestYear: 2025,
                cityName: "Basel",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI", "XX"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
            await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
            await admin.Then_the_created_contest_should_match(contestFormat: "Liverpool",
                contestYear: 2025,
                cityName: "Basel",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI", "XX"],
                completed: false);
        }

        [Test]
        [Arguments("v0.1")]
        [Arguments("v0.2")]
        public async Task Should_create_contest_scenario_2(string apiVersion)
        {
            AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_contest_for_my_countries(contestFormat: "Stockholm",
                contestYear: 2016,
                cityName: "Stockholm",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
            await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
            await admin.Then_the_created_contest_should_match(contestFormat: "Stockholm",
                contestYear: 2016,
                cityName: "Stockholm",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"],
                completed: false);
        }
    }

    private sealed class AdminActor : AdminActorWithResponse<CreateContestResponse>
    {
        public AdminActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Dictionary<string, Guid> countries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            foreach (var (countryCode, countryId) in countries)
            {
                MyCountryCodesAndIds.Add(countryCode, countryId);
            }
        }

        public void Given_I_want_to_create_a_contest_for_my_countries(string[] countryCodes = null!,
            string contestFormat = "",
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] countryIds = countryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            CreateContestRequest requestBody = new()
            {
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                CityName = cityName,
                ContestYear = contestYear,
                ParticipatingCountryIds = countryIds
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public async Task Then_the_created_contest_should_match(string[] countryCodes = null!,
            string contestFormat = "",
            string cityName = "",
            int contestYear = 0,
            bool completed = true)
        {
            CreateContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Contest createdContest = responseBody.Contest;

            Guid[] expectedParticipatingCountryIds = countryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();
            IEnumerable<Guid> actualParticipantCountryIds = createdContest.Participants.Select(p => p.ParticipatingCountryId);

            await Assert.That(createdContest)
                .HasMember(contest => contest.ContestFormat).EqualTo(Enum.Parse<ContestFormat>(contestFormat))
                .And.HasMember(contest => contest.ContestYear).EqualTo(contestYear)
                .And.HasMember(contest => contest.CityName).EqualTo(cityName)
                .And.HasMember(contest => contest.Completed).EqualTo(completed);

            await Assert.That(actualParticipantCountryIds)
                .IsEquivalentTo(expectedParticipatingCountryIds, CollectionOrdering.Any);
        }

        public async Task Then_the_created_contest_should_be_retrievable_by_its_ID()
        {
            CreateContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Contest createdContest = responseBody.Contest;
            Contest retrievedContest = await ApiDriver.GetSingleContestAsync(createdContest.Id);

            await Assert.That(retrievedContest)
                .IsEqualTo(createdContest, new ContestEqualityComparer());
        }
    }
}
