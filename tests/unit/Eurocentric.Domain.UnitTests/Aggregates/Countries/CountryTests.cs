using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Countries;

public static class CountryTests
{
    public sealed class FluentBuilder : UnitTest
    {
        private static Func<CountryId> FixedCountryIdProvider =>
            () => CountryId.FromValue(Guid.Parse("68052047-fea4-4d56-b575-dd033be7d076"));

        private static ErrorOr<CountryCode> ArbitraryCountryCode => CountryCode.FromValue("AA");

        private static ErrorOr<CountryName> ArbitraryCountryName => CountryName.FromValue("CountryName");

        [Theory]
        [InlineData("AT", "Austria")]
        [InlineData("BA", "Bosnia & Herzegovina")]
        [InlineData("XX", "Rest of the World")]
        public void Should_return_new_Country_given_valid_args(string countryCode, string countryName)
        {
            // Act
            ErrorOr<Country> errorsOrCountry = Country.Create()
                .WithCountryCode(CountryCode.FromValue(countryCode))
                .WithCountryName(CountryName.FromValue(countryName))
                .Build(FixedCountryIdProvider);

            (bool isError, Country country) = (errorsOrCountry.IsError, errorsOrCountry.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(country);

            Assert.Equal(countryCode, country.CountryCode.Value);
            Assert.Equal(countryName, country.CountryName.Value);
        }

        [Fact]
        public void Should_return_Errors_when_country_code_not_provided()
        {
            // Act
            ErrorOr<Country> errorsOrCountry = Country.Create()
                .WithCountryName(ArbitraryCountryName)
                .Build(FixedCountryIdProvider);

            (bool isError, Country? country, Error firstError) =
                (errorsOrCountry.IsError, errorsOrCountry.Value, errorsOrCountry.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Country code not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_country_name_not_provided()
        {
            // Act
            ErrorOr<Country> errorsOrCountry = Country.Create()
                .WithCountryCode(ArbitraryCountryCode)
                .Build(FixedCountryIdProvider);

            (bool isError, Country? country, Error firstError) =
                (errorsOrCountry.IsError, errorsOrCountry.Value, errorsOrCountry.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Country name not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_given_illegal_country_code_value()
        {
            // Arrange
            const string illegalCountryCodeValue = "!";

            // Act
            ErrorOr<Country> errorsOrCountry = Country.Create()
                .WithCountryCode(CountryCode.FromValue(illegalCountryCodeValue))
                .WithCountryName(ArbitraryCountryName)
                .Build(FixedCountryIdProvider);

            (bool isError, Country? country, Error firstError) =
                (errorsOrCountry.IsError, errorsOrCountry.Value, errorsOrCountry.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal country code value", firstError.Code);
            Assert.Equal("Country code value must be a string of 2 upper-case letters.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryCode", Value: illegalCountryCodeValue });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_country_name_value()
        {
            // Arrange
            const string illegalCountryNameValue = "";

            // Act
            ErrorOr<Country> errorsOrCountry = Country.Create()
                .WithCountryCode(ArbitraryCountryCode)
                .WithCountryName(CountryName.FromValue(illegalCountryNameValue))
                .Build(FixedCountryIdProvider);

            (bool isError, Country? country, Error firstError) =
                (errorsOrCountry.IsError, errorsOrCountry.Value, errorsOrCountry.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal country name value", firstError.Code);
            Assert.Equal("Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryName", Value: illegalCountryNameValue });
        }
    }
}
