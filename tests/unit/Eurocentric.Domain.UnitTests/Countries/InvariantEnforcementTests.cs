using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Countries;

public sealed class InvariantEnforcementTests : UnitTestBase
{
    public sealed class FailOnCountryCodeConflictExtensionMethod : UnitTestBase
    {
        private static Country CreateCountry(CountryCode countryCode, CountryId id)
        {
            CountryName arbitraryCountryName = CountryName.FromValue("CountryName").Value;
            FixedCountryIdGenerator idGenerator = new(id);

            return Country.Create()
                .WithCountryCode(countryCode)
                .WithCountryName(arbitraryCountryName)
                .Build(idGenerator)
                .Value;
        }

        [Fact]
        public void Should_return_country_when_existingCountries_arg_is_empty_list()
        {
            // Arrange
            Country inputCountry = CreateCountry(CountryCodes.At, FixedCountryIds.Id1);

            ErrorOr<Country> sut = inputCountry.ToErrorOr();

            IQueryable<Country> existingCountries = Array.Empty<Country>().AsQueryable();

            // Act
            ErrorOr<Country> result = sut.FailOnCountryCodeConflict(existingCountries);

            var (isError, outputCountry) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.Same(inputCountry, outputCountry);
        }

        [Fact]
        public void Should_return_country_when_no_existing_country_has_same_country_code()
        {
            // Arrange
            Country inputCountry = CreateCountry(CountryCodes.At, FixedCountryIds.Id1);

            ErrorOr<Country> sut = inputCountry.ToErrorOr();

            IQueryable<Country> existingCountries = new List<Country>
            {
                CreateCountry(CountryCodes.Gb, FixedCountryIds.Id2), CreateCountry(CountryCodes.Xx, FixedCountryIds.Id3)
            }.AsQueryable();

            // Act
            ErrorOr<Country> result = sut.FailOnCountryCodeConflict(existingCountries);

            var (isError, outputCountry) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.Same(inputCountry, outputCountry);
        }

        [Fact]
        public void Should_return_errors_when_existing_country_has_same_country_code()
        {
            // Arrange
            Country inputCountry = CreateCountry(CountryCodes.At, FixedCountryIds.Id1);

            ErrorOr<Country> sut = inputCountry.ToErrorOr();

            IQueryable<Country> existingCountries = new List<Country>
            {
                CreateCountry(CountryCodes.Gb, FixedCountryIds.Id2), CreateCountry(CountryCodes.At, FixedCountryIds.Id3)
            }.AsQueryable();

            // Act
            ErrorOr<Country> result = sut.FailOnCountryCodeConflict(existingCountries);

            var (isError, outputCountry, firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(outputCountry);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Country code conflict", firstError.Code);
            Assert.Equal("A country already exists with the provided country code.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryCode", Value: "AT" });
        }

        [Fact]
        public void Should_return_self_when_instance_is_errors()
        {
            // Arrange
            ErrorOr<Country> sut = ValueObjectErrors.IllegalCountryCodeValue(string.Empty);

            IQueryable<Country> dummyExistingCountries = Array.Empty<Country>().AsQueryable();

            // Act
            ErrorOr<Country> result = sut.FailOnCountryCodeConflict(dummyExistingCountries);

            // Assert
            Assert.Equal(sut, result);
        }

        private static class CountryCodes
        {
            public static readonly CountryCode At = CountryCode.FromValue("AT").Value;
            public static readonly CountryCode Gb = CountryCode.FromValue("Gb").Value;
            public static readonly CountryCode Xx = CountryCode.FromValue("XX").Value;
        }

        private static class FixedCountryIds
        {
            public static readonly CountryId Id1 = CountryId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));
            public static readonly CountryId Id2 = CountryId.FromValue(Guid.Parse("22a5c966-0979-449f-a079-18cd86a3f2c0"));
            public static readonly CountryId Id3 = CountryId.FromValue(Guid.Parse("33ac545c-7bb2-405b-9217-fbaba7d02c58"));
        }
    }
}
