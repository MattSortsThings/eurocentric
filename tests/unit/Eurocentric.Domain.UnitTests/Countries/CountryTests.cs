using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Countries;

public sealed class CountryTests : UnitTestBase
{
    public sealed class AddMemoMethod : UnitTestBase
    {
        private static Country CreateArbitraryCountry()
        {
            CountryId fixedCountryId = CountryId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

            return Country.Create()
                .WithCountryCode(CountryCode.FromValue("AA"))
                .WithCountryName(CountryName.FromValue("CountryName"))
                .Build(new FixedCountryIdGenerator(fixedCountryId))
                .Value;
        }

        [Fact]
        public void Should_update_ParticipatingContests()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("85e25246-bf71-404c-9066-3535a5a152e2"));

            // Assert
            Assert.Empty(sut.ParticipatingContests);

            // Act
            sut.AddMemo(contestId);

            // Assert
            ContestMemo singleMemo = Assert.Single(sut.ParticipatingContests);

            Assert.Equal(contestId, singleMemo.ContestId);
            Assert.Equal(ContestStatus.Initialized, singleMemo.ContestStatus);
        }

        [Fact]
        public void Should_throw_given_ContestId_of_existing_ContestMemo()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("85e25246-bf71-404c-9066-3535a5a152e2"));

            sut.AddMemo(contestId);

            IReadOnlyList<ContestMemo> existingMemos = sut.ParticipatingContests;

            // Act
            Action act = () => sut.AddMemo(contestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemo already exists with the provided ContestId value.", exception.Message);

            Assert.Equal(existingMemos, sut.ParticipatingContests);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            // Act
            Action act = () => sut.AddMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);

            Assert.Empty(sut.ParticipatingContests);
        }
    }

    public sealed class ReplaceMemoMethod : UnitTestBase
    {
        private static Country CreateArbitraryCountry()
        {
            CountryId fixedCountryId = CountryId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

            return Country.Create()
                .WithCountryCode(CountryCode.FromValue("AA"))
                .WithCountryName(CountryName.FromValue("CountryName"))
                .Build(new FixedCountryIdGenerator(fixedCountryId))
                .Value;
        }

        [Fact]
        public void Should_update_ParticipatingContests()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("85e25246-bf71-404c-9066-3535a5a152e2"));

            sut.AddMemo(contestId);

            // Assert
            ContestMemo initialMemo = Assert.Single(sut.ParticipatingContests);
            Assert.Equal(contestId, initialMemo.ContestId);
            Assert.Equal(ContestStatus.Initialized, initialMemo.ContestStatus);

            // Act
            sut.ReplaceMemo(contestId, ContestStatus.InProgress);

            // Assert
            ContestMemo finalMemo = Assert.Single(sut.ParticipatingContests);
            Assert.Equal(contestId, finalMemo.ContestId);
            Assert.Equal(ContestStatus.InProgress, finalMemo.ContestStatus);
        }

        [Fact]
        public void Should_throw_given_ContestId_of_non_existent_ContestMemo()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            ContestId existingContestId = ContestId.FromValue(Guid.Parse("85e25246-bf71-404c-9066-3535a5a152e2"));
            ContestId contestId = ContestId.FromValue(Guid.Parse("d600260e-4b11-4b70-a2dd-ce424a3bd702"));

            sut.AddMemo(existingContestId);

            // Act
            Action act = () => sut.ReplaceMemo(contestId, ContestStatus.InProgress);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("No ContestMemo exists with the provided ContestId value.", exception.Message);

            Assert.Single(sut.ParticipatingContests);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            // Act
            Action act = () => sut.RemoveMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);

            Assert.Empty(sut.ParticipatingContests);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private static Country CreateArbitraryCountry()
        {
            CountryId fixedCountryId = CountryId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

            return Country.Create()
                .WithCountryCode(CountryCode.FromValue("AA"))
                .WithCountryName(CountryName.FromValue("CountryName"))
                .Build(new FixedCountryIdGenerator(fixedCountryId))
                .Value;
        }

        [Fact]
        public void Should_update_ParticipatingContests()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("85e25246-bf71-404c-9066-3535a5a152e2"));

            sut.AddMemo(contestId);

            // Assert
            Assert.Single(sut.ParticipatingContests);

            // Act
            sut.RemoveMemo(contestId);

            // Assert
            Assert.Empty(sut.ParticipatingContests);
        }

        [Fact]
        public void Should_throw_given_ContestId_of_non_existent_ContestMemo()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            ContestId existingContestId = ContestId.FromValue(Guid.Parse("85e25246-bf71-404c-9066-3535a5a152e2"));
            ContestId contestId = ContestId.FromValue(Guid.Parse("d600260e-4b11-4b70-a2dd-ce424a3bd702"));

            sut.AddMemo(existingContestId);

            // Act
            Action act = () => sut.RemoveMemo(contestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("No ContestMemo exists with the provided ContestId value.", exception.Message);

            Assert.Single(sut.ParticipatingContests);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateArbitraryCountry();

            // Act
            Action act = () => sut.RemoveMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);

            Assert.Empty(sut.ParticipatingContests);
        }
    }

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
