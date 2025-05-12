using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Countries;

public sealed class CountryTests : UnitTestBase
{
    public sealed class AddOrReplaceMemoMethod : UnitTestBase
    {
        private static Country CreateCountry() => Country.Create()
            .WithCountryCode(CountryCode.FromValue("AT"))
            .WithName(CountryName.FromValue("Austria"))
            .Build(() => CountryId.FromValue(Guid.Parse("fd0b8faf-7ef8-4530-b20d-f5668bdbbc2e"))).Value;

        [Fact]
        public void Should_add_memo_when_instance_has_no_memo_with_same_contest_ID()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("3e0f25a4-711c-4a62-8bb5-a437a50b3ad7"));
            ContestMemo memoToBeAdded = new(contestId, ContestStatus.Completed);

            // Assert
            Assert.Empty(sut.ContestMemos);

            // Act
            sut.AddOrReplaceMemo(memoToBeAdded);

            // Assert
            Assert.Single(sut.ContestMemos);
            Assert.Contains(memoToBeAdded, sut.ContestMemos);
        }

        [Fact]
        public void Should_replace_memo_when_instance_has_memo_with_same_contest_ID()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("3e0f25a4-711c-4a62-8bb5-a437a50b3ad7"));
            ContestMemo existingMemo = new(contestId, ContestStatus.Completed);
            ContestMemo memoToBeAdded = new(contestId, ContestStatus.InProgress);

            sut.AddOrReplaceMemo(existingMemo);

            // Assert
            Assert.Single(sut.ContestMemos);
            Assert.Contains(existingMemo, sut.ContestMemos);

            // Act
            sut.AddOrReplaceMemo(memoToBeAdded);

            // Assert
            Assert.Single(sut.ContestMemos);
            Assert.Contains(memoToBeAdded, sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_given_null_contestMemo_arg()
        {
            // Arrange
            Country sut = CreateCountry();

            // Act
            Action act = () => sut.AddOrReplaceMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestMemo')", exception.Message);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private static Country CreateCountry() => Country.Create()
            .WithCountryCode(CountryCode.FromValue("AT"))
            .WithName(CountryName.FromValue("Austria"))
            .Build(() => CountryId.FromValue(Guid.Parse("fd0b8faf-7ef8-4530-b20d-f5668bdbbc2e"))).Value;

        [Fact]
        public void Should_remove_memo_when_instance_has_memo_with_provided_contest_ID()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("3e0f25a4-711c-4a62-8bb5-a437a50b3ad7"));
            ContestMemo memoToBeRemoved = new(contestId, ContestStatus.Completed);

            sut.AddOrReplaceMemo(memoToBeRemoved);

            // Assert
            Assert.Single(sut.ContestMemos);
            Assert.Contains(memoToBeRemoved, sut.ContestMemos);

            // Act
            sut.RemoveMemo(contestId);

            // Assert
            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_when_instance_has_no_memo_with_provided_contest_ID()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(Guid.Parse("3e0f25a4-711c-4a62-8bb5-a437a50b3ad7"));

            // Assert
            Assert.Empty(sut.ContestMemos);

            // Act
            Action act = () => sut.RemoveMemo(contestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("No contest memo present with contest ID 3e0f25a4-711c-4a62-8bb5-a437a50b3ad7.", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateCountry();

            // Act
            Action act = () => sut.RemoveMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);
        }
    }

    public sealed class FluentBuilder : UnitTestBase
    {
        private static readonly ErrorOr<CountryCode> ArbitraryCountryCode = CountryCode.FromValue("AA");
        private static readonly ErrorOr<CountryName> ArbitraryCountryName = CountryName.FromValue("CountryName");

        private static readonly CountryId FixedCountryId =
            CountryId.FromValue(Guid.Parse("989c773a-0306-4136-813d-c10d8de2935b"));

        [Theory]
        [InlineData("AT", "Austria")]
        [InlineData("BA", "Bosnia & Herzegovina")]
        [InlineData("XX", "Rest of the World")]
        public void Should_create_country_with_provided_values_and_empty_contest_memos(string countryCode, string countryName)
        {
            // Act
            ErrorOr<Country> errorsOrResult = Country.Create()
                .WithCountryCode(CountryCode.FromValue(countryCode))
                .WithName(CountryName.FromValue(countryName))
                .Build(() => FixedCountryId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);
            Assert.Equal(FixedCountryId, result.Id);
            Assert.Equal(countryCode, result.CountryCode.Value);
            Assert.Equal(countryName, result.Name.Value);
            Assert.Empty(result.ContestMemos);
        }

        [Fact]
        public void Should_return_errors_given_illegal_country_code_value()
        {
            // Arrange
            ErrorOr<CountryCode> illegalCountryCode = CountryCode.FromValue(string.Empty);

            // Act
            ErrorOr<Country> errorsOrResult = Country.Create()
                .WithCountryCode(illegalCountryCode)
                .WithName(ArbitraryCountryName)
                .Build(() => FixedCountryId);

            (bool isError, Country result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal country code value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_illegal_country_name_value()
        {
            // Arrange
            ErrorOr<CountryName> illegalCountryName = CountryName.FromValue(string.Empty);

            // Act
            ErrorOr<Country> errorsOrResult = Country.Create()
                .WithCountryCode(ArbitraryCountryCode)
                .WithName(illegalCountryName)
                .Build(() => FixedCountryId);

            (bool isError, Country result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal country name value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Act
            Action act = () => Country.Create()
                .WithCountryCode(ArbitraryCountryCode)
                .WithName(ArbitraryCountryName)
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }
}
