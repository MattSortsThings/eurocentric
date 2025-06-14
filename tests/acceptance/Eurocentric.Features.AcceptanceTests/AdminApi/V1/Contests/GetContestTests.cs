using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Lisbon",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_retrieve_non_existent_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Lisbon",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_contest();
        admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key("contestId");
    }

    private sealed class AdminActor : ActorWithResponse<GetContestResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(6);

        private Contest? MyContest { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest(int contestYear = 0,
            string cityName = "",
            string contestFormat = "",
            string? group0CountryCode = null,
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null) => MyContest = await ApiDriver.Contests.CreateAContestAsync(cityName: cityName,
            contestFormat: Enum.Parse<ContestFormat>(contestFormat),
            contestYear: contestYear,
            group0CountryId: group0CountryCode is null ? null : MyCountryCodesAndIds[group0CountryCode],
            group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
            group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

        public async Task Given_I_have_deleted_my_contest()
        {
            Assert.NotNull(MyContest);

            await ApiDriver.Contests.DeleteAContestAsync(MyContest.Id, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_retrieve_my_contest_by_its_ID()
        {
            Assert.NotNull(MyContest);

            Guid contestId = MyContest.Id;

            SendMyRequest = apiDriver => apiDriver.Contests.GetContest(contestId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_contest_should_be_my_contest()
        {
            Assert.NotNull(ResponseObject);
            Assert.NotNull(MyContest);

            Assert.Equal(MyContest, ResponseObject.Contest, new ContestEqualityComparer());
        }

        public void Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key(string key)
        {
            Assert.NotNull(MyContest);

            Then_the_problem_details_extensions_should_contain(key, MyContest.Id);
        }
    }
}
