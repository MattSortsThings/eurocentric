using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Contests.GetContest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public static class GetContestTest
{
    public sealed class GetContest : SerialCleanAcceptanceTest
    {
        [Test]
        [Arguments("v0.1")]
        [Arguments("v0.2")]
        public async Task Should_retrieve_requested_contest(string apiVersion)
        {
            AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
                cityName: "Basel",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);

            await admin.Given_I_want_to_retrieve_my_contest();

            // When
            await admin.When_I_send_my_request();

            // Then
            await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
            await admin.Then_the_retrieved_contest_should_be_my_contest();
        }
    }

    private sealed class AdminActor : AdminActorWithResponse<GetContestResponse>
    {
        public AdminActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Contest? MyContest { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Dictionary<string, Guid> countries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            foreach (var (countryCode, countryId) in countries)
            {
                MyCountryCodesAndIds.Add(countryCode, countryId);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] countryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] countryIds = countryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            MyContest = await ApiDriver.CreateSingleContestAsync(contestFormat: ContestFormat.Stockholm,
                cityName: cityName,
                contestYear: contestYear,
                countryIds: countryIds);
        }

        public async Task Given_I_want_to_retrieve_my_contest()
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.GetContest(myContest.Id);
        }

        public async Task Then_the_retrieved_contest_should_be_my_contest()
        {
            Contest expectedContest = await Assert.That(MyContest).IsNotNull();
            GetContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Contest)
                .IsEqualTo(expectedContest, new ContestEqualityComparer());
        }
    }
}
