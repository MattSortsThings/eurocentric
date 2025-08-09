using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Countries;

public sealed class CountryTests : UnitTest
{
    private static readonly CountryCode ArbitraryCountryCode = CountryCode.FromValue("AA").Value;
    private static readonly CountryName ArbitraryCountryName = CountryName.FromValue("CountryName").Value;
    private static readonly CountryId FixedCountryId = CountryId.FromValue(Guid.Parse("24c201bc-709a-4b37-a31e-f193d9832244"));

    [Test]
    [Arguments("AT", "Austria", "bd9c7868-df7a-4492-b5c1-82d7585c8ad4")]
    [Arguments("BA", "Bosnia & Herzegovina", "7f6df94a-6c76-42b1-910c-18d8b1eb3807")]
    [Arguments("CH", "Switzerland", "4027bdce-4f88-4494-8425-315b8af69a39")]
    [Arguments("GB", "United Kingdom", "84b588b4-5c4c-4f0b-8260-94c37fefc535")]
    [Arguments("XX", "Rest of the World", "f07255b4-dc46-4400-be69-42eaf88df855")]
    public async Task FluentBuilder_should_return_Country_with_provided_CountryCode_and_CountryName_and_ID(string countryCode,
        string countryName,
        string countryIdValue)
    {
        // Arrange
        ErrorOr<CountryCode> errorsOrCountryCode = CountryCode.FromValue(countryCode);
        ErrorOr<CountryName> errorsOrCountryName = CountryName.FromValue(countryName);
        Guid countryId = Guid.Parse(countryIdValue);

        // Act
        ErrorOr<Country> result = Country.Create()
            .WithCountryCode(errorsOrCountryCode)
            .WithCountryName(errorsOrCountryName)
            .Build(() => CountryId.FromValue(countryId));

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Country country = await Assert.That(result.Value).IsNotNull();

        await Assert.That(country.Id).HasMember(id => id.Value).EqualTo(countryId);
        await Assert.That(country.CountryCode).HasMember(id => id.Value).EqualTo(countryCode);
        await Assert.That(country.CountryName).HasMember(name => name.Value).EqualTo(countryName);
    }

    [Test]
    public async Task FluentBuilder_should_return_Country_with_empty_ParticipatingCountryIds()
    {
        // Act
        ErrorOr<Country> result = Country.Create()
            .WithCountryCode(ArbitraryCountryCode)
            .WithCountryName(ArbitraryCountryName)
            .Build(() => FixedCountryId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Country country = await Assert.That(result.Value).IsNotNull();

        await Assert.That(country.ParticipatingContestIds).IsEmpty();
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_when_country_code_not_provided()
    {
        // Arrange
        Func<CountryId> dummyIdProvider = () => CountryId.FromValue(Guid.Empty);

        // Act
        ErrorOr<Country> result = Country.Create()
            .WithCountryName(ArbitraryCountryName)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Unexpected)
            .And.HasCode("Country code not set")
            .And.HasDescription("Country builder invoked without setting country code.");
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_when_country_name_not_provided()
    {
        // Arrange
        Func<CountryId> dummyIdProvider = () => CountryId.FromValue(Guid.Empty);

        // Act
        ErrorOr<Country> result = Country.Create()
            .WithCountryCode(ArbitraryCountryCode)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Unexpected)
            .And.HasCode("Country name not set")
            .And.HasDescription("Country builder invoked without setting country name.");
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_given_illegal_country_code_value()
    {
        // Arrange
        const string countryCodeValue = "!";
        Func<CountryId> dummyIdProvider = () => CountryId.FromValue(Guid.Empty);

        // Act
        ErrorOr<Country> result = Country.Create()
            .WithCountryCode(CountryCode.FromValue(countryCodeValue))
            .WithCountryName(ArbitraryCountryName)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal country code value")
            .And.HasDescription("Country code value must be a string of 2 upper-case letters.")
            .And.HasMetadataEntry("countryCode", countryCodeValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_given_illegal_country_name_value()
    {
        // Arrange
        const string countryNameValue = " ";
        Func<CountryId> dummyIdProvider = () => CountryId.FromValue(Guid.Empty);

        // Act
        ErrorOr<Country> result = Country.Create()
            .WithCountryCode(ArbitraryCountryCode)
            .WithCountryName(CountryName.FromValue(countryNameValue))
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal country name value")
            .And.HasDescription("Country name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("countryName", countryNameValue);
    }

    [Test]
    public async Task FluentBuilder_should_throw_given_null_idProvider_arg()
    {
        // Act
        Action act = () => Country.Create()
            .WithCountryCode(ArbitraryCountryCode)
            .WithCountryName(ArbitraryCountryName)
            .Build(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'idProvider')")
            .WithParameterName("idProvider");
    }
}
