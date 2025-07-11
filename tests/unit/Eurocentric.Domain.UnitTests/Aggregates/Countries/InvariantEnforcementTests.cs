using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Countries;

public static class InvariantEnforcementTests
{
    public sealed class FailOnCountryCodeConflictExtensionMethod : UnitTest
    {
        private static readonly CountryId
            CountryId1Of3 = CountryId.FromValue(Guid.Parse("11052047-fea4-4d56-b575-dd033be7d076"));
        private static readonly CountryId
            CountryId2Of3 = CountryId.FromValue(Guid.Parse("22fe8e6c-bf26-4d5b-9f28-b0695b6035f8"));
        private static readonly CountryId
            CountryId3Of3 = CountryId.FromValue(Guid.Parse("3344f215-56e5-437b-b310-d933118f9b4d"));

        private static ErrorOr<CountryCode> ArbitraryCountryCode => CountryCode.FromValue("AA");

        private static ErrorOr<CountryName> ArbitraryCountryName => CountryName.FromValue("CountryName");

        private static Country CreateCountry(ErrorOr<CountryCode> countryCode, CountryId countryId) => Country.Create()
            .WithCountryCode(countryCode)
            .WithCountryName(ArbitraryCountryName)
            .Build(() => countryId)
            .Value;

        [Fact]
        public void Should_return_instance_when_instance_is_Country_and_country_code_is_unique()
        {
            // Arrange
            ErrorOr<Country> sut = CreateCountry(CountryCode.FromValue("AT"), CountryId1Of3).ToErrorOr();

            List<Country> existingCountries =
            [
                CreateCountry(CountryCode.FromValue("BE"), CountryId2Of3),
                CreateCountry(CountryCode.FromValue("XX"), CountryId3Of3)
            ];

            // Act
            ErrorOr<Country> errorsOrCountry = sut.FailOnCountryCodeConflict(existingCountries.AsQueryable());

            (bool isError, Country country) = (errorsOrCountry.IsError, errorsOrCountry.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(country);
        }

        [Fact]
        public void Should_return_Errors_when_instance_is_Country_and_country_code_is_not_unique()
        {
            // Arrange
            ErrorOr<Country> sut = CreateCountry(CountryCode.FromValue("AT"), CountryId1Of3).ToErrorOr();

            List<Country> existingCountries =
            [
                CreateCountry(CountryCode.FromValue("AT"), CountryId2Of3),
                CreateCountry(CountryCode.FromValue("XX"), CountryId3Of3)
            ];

            // Act
            ErrorOr<Country> errorsOrCountry = sut.FailOnCountryCodeConflict(existingCountries.AsQueryable());

            (bool isError, Country country, Error firstError) =
                (errorsOrCountry.IsError, errorsOrCountry.Value, errorsOrCountry.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(country);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Country code conflict", firstError.Code);
            Assert.Equal("A country already exists with the provided country code.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryCode", Value: "AT" });
        }

        [Fact]
        public void Should_return_Errors_when_instance_is_Errors()
        {
            // Arrange
            ErrorOr<Country> sut = Error.Failure("Failure");

            // Act
            ErrorOr<Country> errorsOrCountry = sut.FailOnCountryCodeConflict(Enumerable.Empty<Country>().AsQueryable());

            // Assert
            Assert.Equal(sut, errorsOrCountry);
        }

        [Fact]
        public void Should_throw_given_null_existingCountries_arg()
        {
            // Arrange
            ErrorOr<Country> sut = Error.Failure("Failure");

            // Act
            Action act = () => sut.FailOnCountryCodeConflict(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'existingCountries')", exception.Message);
        }
    }
}
