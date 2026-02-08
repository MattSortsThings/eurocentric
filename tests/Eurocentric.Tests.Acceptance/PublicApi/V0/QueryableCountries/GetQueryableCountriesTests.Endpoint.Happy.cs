using Eurocentric.Apis.Public.V0.QueryableCountries;
using Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.QueryableCountries;

[Repeat(30)]
[NotInParallel("PublicApi.V0.GetQueryableCountries")]
public sealed class GetQueryableCountriesTests_Endpoint : AcceptanceTests
{
    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_succeed_with_200_OK_and_all_queryable_countries_in_country_code_order(string apiVersion)
    {
        HappyEuroFan euroFan = EuroFan
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingDemoApiKey()
            .Expecting200OK<GetQueryableCountriesResponseBody>()
            .Build(HappyEuroFan.Create);

        // Given
        await euroFan.Given_the_system_contains_all_53_countries();
        await euroFan.Given_the_system_contains_these_contests(
            """
            | contest_year | completed_contest_stages           |
            |-------------:|:-----------------------------------|
            |         2021 | GrandFinal                         |
            |         2022 | SemiFinal1, SemiFinal2, GrandFinal |
            |         2023 | SemiFinal1, SemiFinal2, GrandFinal |
            |         2024 | SemiFinal1                         |
            """
        );
        euroFan.Given_I_want_to_list_the_queryable_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_succeed_with_status_code_200_OK();
        await euroFan.Then_the_queryable_countries_should_be_in_country_code_order();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_succeed_with_empty_queryable_countries_list_when_no_queryable_voting_data_exists(
        string apiVersion
    )
    {
        HappyEuroFan euroFan = EuroFan
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingDemoApiKey()
            .Expecting200OK<GetQueryableCountriesResponseBody>()
            .Build(HappyEuroFan.Create);

        // Given
        await euroFan.Given_the_system_contains_all_53_countries();
        await euroFan.Given_the_system_contains_these_contests(
            """
            | contest_year | completed_contest_stages |
            |-------------:|:-------------------------|
            |         2024 | SemiFinal1               |
            """
        );
        euroFan.Given_I_want_to_list_the_queryable_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_succeed_with_status_code_200_OK();
        await euroFan.Then_the_queryable_countries_list_should_be_empty();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_succeed_with_empty_queryable_countries_list_when_system_contains_no_data(string apiVersion)
    {
        HappyEuroFan euroFan = EuroFan
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingDemoApiKey()
            .Expecting200OK<GetQueryableCountriesResponseBody>()
            .Build(HappyEuroFan.Create);

        // Given
        euroFan.Given_I_want_to_list_the_queryable_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_succeed_with_status_code_200_OK();
        await euroFan.Then_the_queryable_countries_list_should_be_empty();
    }

    private sealed class HappyEuroFan : EuroFanExpecting200OK<GetQueryableCountriesResponseBody>
    {
        private HappyEuroFan(IEuroFanKernel kernel)
            : base(kernel) { }

        public void Given_I_want_to_list_the_queryable_countries() =>
            Request = Kernel.RequestFactory.GetQueryableCountries();

        public static HappyEuroFan Create(IEuroFanKernel kernel) => new(kernel);

        public async Task Then_the_queryable_countries_should_be_in_country_code_order()
        {
            await Assert
                .That(SuccessResponse?.Data?.QueryableCountries)
                .IsNotEmpty()
                .And.IsOrderedBy(country => country.CountryCode);
        }

        public async Task Then_the_queryable_countries_list_should_be_empty() =>
            await Assert.That(SuccessResponse?.Data?.QueryableCountries).IsEmpty();
    }
}
