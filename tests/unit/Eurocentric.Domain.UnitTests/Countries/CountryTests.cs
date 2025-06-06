using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Countries.Utilities;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Countries;

public sealed class CountryTests : UnitTestBase
{
    public sealed class FluentBuilder : UnitTestBase
    {
        private static readonly CountryId FixedCountryId =
            CountryId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

        private static readonly CountryCode ArbitraryCountryCode = CountryCode.FromValue("AA").Value;
        private static readonly CountryName ArbitraryCountryName = CountryName.FromValue("CountryName").Value;

        [Theory]
        [InlineData("AT", "Austria")]
        [InlineData("BA", "Bosnia & Herzegovina")]
        [InlineData("CH", "Switzerland")]
        [InlineData("GB", "United Kingdom")]
        [InlineData("XX", "Rest of the World")]
        public void Should_create_country_given_legal_args(string countryCode, string countryName)
        {
            // Arrange
            FixedCountryIdGenerator idGenerator = new(FixedCountryId);

            // Act
            ErrorOr<Country> result = Country.Create()
                .WithCountryCode(CountryCode.FromValue(countryCode))
                .WithCountryName(CountryName.FromValue(countryName))
                .Build(idGenerator);

            (bool isError, Country country) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(country);

            Assert.Equal(FixedCountryId, country.Id);
            Assert.Equal(countryCode, country.CountryCode.Value);
            Assert.Equal(countryName, country.CountryName.Value);
            Assert.Empty(country.ParticipatingContests);
        }

        [Fact]
        public void Should_return_errors_given_illegal_country_code_value()
        {
            // Arrange
            FixedCountryIdGenerator dummyIdGenerator = new(FixedCountryId);

            const string illegalCountryCodeValue = "0";

            // Act
            ErrorOr<Country> result = Country.Create()
                .WithCountryCode(CountryCode.FromValue(illegalCountryCodeValue))
                .WithCountryName(ArbitraryCountryName)
                .Build(dummyIdGenerator);

            // Assert
            var (isError, country, firstError) = (result.IsError, result.Value, result.FirstError);

            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal country code value", firstError.Code);
            Assert.Equal("Country code value must be a string of 2 upper-case letters.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryCode", Value: illegalCountryCodeValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_country_name_value()
        {
            // Arrange
            FixedCountryIdGenerator dummyIdGenerator = new(FixedCountryId);

            const string illegalCountryNameValue = "";

            // Act
            ErrorOr<Country> result = Country.Create()
                .WithCountryCode(ArbitraryCountryCode)
                .WithCountryName(CountryName.FromValue(illegalCountryNameValue))
                .Build(dummyIdGenerator);

            // Assert
            var (isError, country, firstError) = (result.IsError, result.Value, result.FirstError);

            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal country name value", firstError.Code);
            Assert.Equal("Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryName", Value: illegalCountryNameValue });
        }

        [Fact]
        public void Should_throw_given_null_idGenerator_arg()
        {
            // Act
            Action act = () => Country.Create()
                .WithCountryCode(ArbitraryCountryCode)
                .WithCountryName(ArbitraryCountryName)
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);

            Assert.Equal("Value cannot be null. (Parameter 'idGenerator')", exception.Message);
        }
    }
}
