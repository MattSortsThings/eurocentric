using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;
using NSubstitute;

namespace Eurocentric.UnitTests.Domain.Aggregates.Countries;

public sealed class CountryTests : UnitTest
{
    private static readonly CountryId DefaultCountryId = CountryId.FromValue(
        Guid.Parse("fe901f2e-35ab-4c3a-a828-8d0fd019a182")
    );

    private static readonly CountryCode DefaultCountryCode = CountryCode.FromValue("AA").GetValueOrDefault();
    private static readonly CountryName DefaultCountryName = CountryName.FromValue("CountryName").GetValueOrDefault();

    [Test]
    [Arguments("26786949-965a-44cc-801a-22c6b5667a3b")]
    [Arguments("cbafd594-551e-4d87-8089-a8450d4ad059")]
    public async Task Builder_should_create_Country_with_specified_ID(string countryIdValue)
    {
        // Arrange
        CountryId countryId = CountryId.FromValue(Guid.Parse(countryIdValue));

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => countryId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(country => country.Id, source => source.IsEqualTo(countryId));
    }

    [Test]
    [Arguments("AT")]
    [Arguments("SM")]
    [Arguments("XX")]
    public async Task Builder_should_create_Country_with_specified_CountryCode(string countryCodeValue)
    {
        // Arrange
        CountryCode countryCode = CountryCode.FromValue(countryCodeValue).GetValueOrDefault();

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(countryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(country => country.CountryCode, source => source.IsEqualTo(countryCode));
    }

    [Test]
    [Arguments("Austria")]
    [Arguments("San Marino")]
    [Arguments("Rest of the World")]
    public async Task Builder_should_create_Country_with_specified_CountryName(string countryNameValue)
    {
        // Arrange
        CountryName countryName = CountryName.FromValue(countryNameValue).GetValueOrDefault();

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(countryName)
            .Build(() => DefaultCountryId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(country => country.CountryName, source => source.IsEqualTo(countryName));
    }

    [Test]
    public async Task Builder_should_create_Country_with_empty_ContestRoles()
    {
        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(country => country.ContestRoles, source => source.HasProperty(list => list.Count, 0));
    }

    [Test]
    public async Task Builder_should_fail_when_CountryCode_not_set()
    {
        // Arrange
        ICountryIdFactory idFactorySpy = Substitute.For<ICountryIdFactory>();

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryName(DefaultCountryName)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("CountryCode not set")
            .And.HasDetail("Client attempted to create a Country aggregate without setting its CountryCode property.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_when_CountryName_not_set()
    {
        // Arrange
        ICountryIdFactory idFactorySpy = Substitute.For<ICountryIdFactory>();

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("CountryName not set")
            .And.HasDetail("Client attempted to create a Country aggregate without setting its CountryName property.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_given_illegal_CountryCode_value()
    {
        // Arrange
        ICountryIdFactory idFactorySpy = Substitute.For<ICountryIdFactory>();

        const string countryCodeValue = " ";

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(CountryCode.FromValue(countryCodeValue))
            .WithCountryName(DefaultCountryName)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country code value")
            .And.HasDetail("Country code value must be a string of 2 upper-case letters.")
            .And.HasExtension("countryCode", countryCodeValue);
    }

    [Test]
    public async Task Builder_should_fail_given_illegal_CountryName_value()
    {
        // Arrange
        ICountryIdFactory idFactorySpy = Substitute.For<ICountryIdFactory>();

        const string countryNameValue = " ";

        // Act
        Result<Country, IDomainError> result = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(CountryName.FromValue(countryNameValue))
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country name value")
            .And.HasDetail(
                "Country name value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("countryName", countryNameValue);
    }

    [Test]
    public async Task Builder_Build_should_throw_given_null_idProvider_value()
    {
        // Assert
        await Assert
            .That(() => Country.Create().Build(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'idProvider')");
    }
}
