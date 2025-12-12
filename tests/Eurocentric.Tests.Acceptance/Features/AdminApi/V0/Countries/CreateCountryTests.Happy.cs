using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Apis.Admin.V0.Common.Enums;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

public sealed partial class CreateCountryTests
{
    [Test]
    [V0Point1Upward]
    public async Task CreateCountry_should_create_country_scenario_1(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithResponse<CreateCountryResponse>()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        actor.Given_I_want_to_create_a_country(countryCode: "AT", countryName: "Austria");

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(201);
        await actor.Then_the_response_headers_should_include_the_created_country_location_for_API_version(apiVersion);
        await actor.Then_the_created_country_should_match(countryCode: "AT", countryName: "Austria");
        await actor.Then_the_created_country_should_have_no_contest_roles();
        await actor.Then_the_created_country_should_exist();
    }

    [Test]
    [V0Point1Upward]
    public async Task CreateCountry_should_create_country_scenario_2(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithResponse<CreateCountryResponse>()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        actor.Given_I_want_to_create_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(201);
        await actor.Then_the_response_headers_should_include_the_created_country_location_for_API_version(apiVersion);
        await actor.Then_the_created_country_should_match(countryCode: "BA", countryName: "Bosnia & Herzegovina");
        await actor.Then_the_created_country_should_have_no_contest_roles();
        await actor.Then_the_created_country_should_exist();
    }

    [Test]
    [V0Point1Upward]
    public async Task CreateCountry_should_create_country_scenario_3(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithResponse<CreateCountryResponse>()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        actor.Given_I_want_to_create_a_country(countryCode: "XX", countryName: "Rest of the World");

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(201);
        await actor.Then_the_response_headers_should_include_the_created_country_location_for_API_version(apiVersion);
        await actor.Then_the_created_country_should_match(countryCode: "XX", countryName: "Rest of the World");
        await actor.Then_the_created_country_should_have_no_contest_roles();
        await actor.Then_the_created_country_should_exist();
    }

    private sealed class HappyPathActor : FeatureActor
    {
        private HappyPathActor(IActorKernel kernel)
            : base(kernel) { }

        public static HappyPathActor Create(IActorKernel kernel) => new(kernel);

        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "")
        {
            Request = Kernel.RestRequestFactory.CreateCountry(
                new CreateCountryRequest
                {
                    CountryType = CountryType.Real,
                    CountryCode = countryCode,
                    CountryName = countryName,
                }
            );
        }

        public async Task Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            await Assert
                .That(SuccessResponse?.Data?.Country)
                .HasProperty(country => country.CountryCode, countryCode)
                .And.HasProperty(country => country.CountryName, countryName);
        }

        public async Task Then_the_created_country_should_have_no_contest_roles() =>
            await Assert.That(SuccessResponse?.Data?.Country.ContestRoles).IsEmpty();

        public async Task Then_the_created_country_should_exist()
        {
            Country createdCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert.That(existingCountries).Contains(createdCountry, EqualityComparer<Country>.Default);
        }

        public async Task Then_the_response_headers_should_include_the_created_country_location_for_API_version(
            string apiVersion
        )
        {
            Guid createdCountryId = await Assert.That(SuccessResponse?.Data?.Country.Id).IsNotNull();

            string expectedLocationSuffix = $"/admin/api/{apiVersion}/countries/{createdCountryId}";

            await Assert
                .That(SuccessResponse?.Headers)
                .Contains(headerParameter =>
                    headerParameter.Name == "Location" && headerParameter.Value.EndsWith(expectedLocationSuffix)
                );
        }
    }
}
