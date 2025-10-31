using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class HandleContestCreatedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_participating_and_voting_countries_on_Liverpool_rules_contest_created(
        string apiVersion
    )
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_for_my_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["FI", "GB", "HR"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_existing_countries_should_be_updated_with_contest_roles_in_the_created_contest(
            countriesWithGlobalTelevoteRole: ["XX"],
            countriesWithParticipantRole: ["AT", "BE", "CZ", "FI", "GB", "HR"],
            countriesNotUpdated: ["DK", "EE", "IT"]
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_participating_countries_on_Stockholm_rules_contest_created(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_Stockholm_rules_contest_for_my_countries(
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["FI", "GB", "HR"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_existing_countries_should_be_updated_with_contest_roles_in_the_created_contest(
            countriesWithParticipantRole: ["AT", "BE", "CZ", "FI", "GB", "HR"],
            countriesNotUpdated: ["DK", "EE", "IT", "XX"]
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_not_update_any_country_on_failed_Liverpool_rules_contest_creation(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_for_my_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "AT", "AT"],
            semiFinal2Countries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_every_existing_country_should_have_no_contest_roles();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_not_update_any_country_on_failed_Stockholm_rules_contest_creation(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_Stockholm_rules_contest_for_my_countries(
            semiFinal1Countries: ["AT", "AT", "AT"],
            semiFinal2Countries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_every_existing_country_should_have_no_contest_roles();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<CreateContestResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public void Given_I_want_to_create_a_Liverpool_rules_contest_for_my_countries(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string globalTelevoteCountry = ""
        )
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                ContestRules = ContestRules.Liverpool,
                GlobalTelevoteVotingCountryId = ExistingCountryIds.GetId(globalTelevoteCountry),
                Participants = semiFinal1Countries
                    .Select(ExistingCountryIds.GetId)
                    .Select(TestDefaults.SemiFinal1ParticipantRequest)
                    .Concat(
                        semiFinal2Countries
                            .Select(ExistingCountryIds.GetId)
                            .Select(TestDefaults.SemiFinal2ParticipantRequest)
                    )
                    .ToArray(),
            };

            Request = Kernel.Requests.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_rules_contest_for_my_countries(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!
        )
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                ContestRules = ContestRules.Stockholm,
                GlobalTelevoteVotingCountryId = null,
                Participants = semiFinal1Countries
                    .Select(ExistingCountryIds.GetId)
                    .Select(TestDefaults.SemiFinal1ParticipantRequest)
                    .Concat(
                        semiFinal2Countries
                            .Select(ExistingCountryIds.GetId)
                            .Select(TestDefaults.SemiFinal2ParticipantRequest)
                    )
                    .ToArray(),
            };

            Request = Kernel.Requests.Contests.CreateContest(requestBody);
        }

        public async Task Then_the_existing_countries_should_be_updated_with_contest_roles_in_the_created_contest(
            string[]? countriesNotUpdated = null,
            string[]? countriesWithParticipantRole = null,
            string[]? countriesWithGlobalTelevoteRole = null
        )
        {
            Guid contestId = await Assert.That(SuccessResponse?.Data?.Contest.Id).IsNotNull();
            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await AssertAllPassedAsync(
                existingCountries,
                MapToSelectionPredicate(countriesNotUpdated),
                HasNoContestRoles
            );

            await AssertAllPassedAsync(
                existingCountries,
                MapToSelectionPredicate(countriesWithParticipantRole),
                HasSingleContestRole(contestId: contestId, roleType: ContestRoleType.Participant)
            );

            await AssertAllPassedAsync(
                existingCountries,
                MapToSelectionPredicate(countriesWithGlobalTelevoteRole),
                HasSingleContestRole(contestId: contestId, roleType: ContestRoleType.GlobalTelevote)
            );
        }

        private Func<Country, bool> MapToSelectionPredicate(string[]? countryCodes)
        {
            Guid[] selectedCountryIds = countryCodes is not null ? ExistingCountryIds.MapToGuids(countryCodes) : [];

            return country => selectedCountryIds.Contains(country.Id);
        }

        private static async Task AssertAllPassedAsync(
            Country[] allCountries,
            Func<Country, bool> selectionPredicate,
            Func<Country, bool> testPredicate
        )
        {
            Country[] selectedCountries = allCountries.Where(selectionPredicate).ToArray();
            Country[] excludedCountries = allCountries.Except(selectedCountries).ToArray();

            await Assert.That(selectedCountries).ContainsOnly(testPredicate).Or.IsEmpty();
            await Assert.That(excludedCountries).DoesNotContain(testPredicate).Or.IsEmpty();
        }

        public async Task Then_every_existing_country_should_have_no_contest_roles()
        {
            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert.That(existingCountries).ContainsOnly(HasNoContestRoles);
        }

        private static bool HasNoContestRoles(Country country) => country.ContestRoles.Length == 0;

        private static Func<Country, bool> HasSingleContestRole(
            ContestRoleType roleType = default,
            Guid contestId = default
        )
        {
            return country =>
                country.ContestRoles.Length == 1
                && country.ContestRoles.Single() is { } role
                && role.ContestId.Equals(contestId)
                && role.ContestRoleType == roleType;
        }
    }
}
