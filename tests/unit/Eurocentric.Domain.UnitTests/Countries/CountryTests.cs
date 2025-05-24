using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Countries;

public sealed class CountryTests : UnitTestBase
{
    private static readonly CountryId FixedCountryId =
        CountryId.FromValue(Guid.Parse("989c773a-0306-4136-813d-c10d8de2935b"));

    public sealed class AddMemoMethod : UnitTestBase
    {
        private static Country InitializeCountry() => Country.Create()
            .WithCountryCode(CountryCode.FromValue("AA"))
            .WithName(CountryName.FromValue("CountryName"))
            .Build(() => FixedCountryId)
            .Value;

        [Fact]
        public void Should_add_memo_with_provided_contest_ID_value_and_Initialized_status()
        {
            // Arrange
            Country sut = InitializeCountry();

            ContestId contestId = ContestIds.GetOne();

            // Assert
            Assert.Empty(sut.ContestMemos);

            // Act
            sut.AddMemo(contestId);

            // Assert
            ContestMemo memo = Assert.Single(sut.ContestMemos);

            Assert.Equal(contestId, memo.ContestId);
            Assert.Equal(ContestStatus.Initialized, memo.Status);
        }

        [Fact]
        public void Should_throw_when_has_memo_with_provided_contest_ID_value()
        {
            // Arrange
            Country sut = InitializeCountry();

            ContestId contestId = ContestIds.GetOne();

            sut.AddMemo(contestId);

            // Assert
            ContestMemo initialMemo = Assert.Single(sut.ContestMemos);

            // Act
            Action act = () => sut.AddMemo(contestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemos collection contains an item with the provided ContestId value.", exception.Message);

            ContestMemo memo = Assert.Single(sut.ContestMemos);
            Assert.Equal(initialMemo, memo);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = InitializeCountry();

            // Act
            Action act = () => sut.AddMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);
        }
    }

    public sealed class ReplaceMemoMethod : UnitTestBase
    {
        private static Country InitializeCountry() => Country.Create()
            .WithCountryCode(CountryCode.FromValue("AA"))
            .WithName(CountryName.FromValue("CountryName"))
            .Build(() => FixedCountryId)
            .Value;

        [Fact]
        public void Should_replace_memo_with_provided_contest_ID_and_status_values()
        {
            // Arrange
            Country sut = InitializeCountry();

            var (contestId1Of2, contestId2Of2) = ContestIds.GetTwo();

            sut.AddMemo(contestId1Of2);
            sut.AddMemo(contestId2Of2);

            // Assert
            Assert.Equivalent((ContestMemo[])
            [
                new ContestMemo(contestId1Of2, ContestStatus.Initialized),
                new ContestMemo(contestId2Of2, ContestStatus.Initialized)
            ], sut.ContestMemos);

            // Act
            sut.ReplaceMemo(contestId2Of2, ContestStatus.InProgress);

            // Assert
            Assert.Equivalent((ContestMemo[])
            [
                new ContestMemo(contestId1Of2, ContestStatus.Initialized),
                new ContestMemo(contestId2Of2, ContestStatus.InProgress)
            ], sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_when_has_no_memo_with_provided_contest_ID_value()
        {
            // Arrange
            Country sut = InitializeCountry();

            ContestId contestId = ContestIds.GetOne();

            // Assert
            Assert.Empty(sut.ContestMemos);

            // Act
            Action act = () => sut.ReplaceMemo(contestId, ContestStatus.InProgress);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemos collection contains no item with the provided ContestId value.", exception.Message);

            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = InitializeCountry();

            // Act
            Action act = () => sut.ReplaceMemo(null!, ContestStatus.InProgress);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private static Country InitializeCountry() => Country.Create()
            .WithCountryCode(CountryCode.FromValue("AA"))
            .WithName(CountryName.FromValue("CountryName"))
            .Build(() => FixedCountryId)
            .Value;

        [Fact]
        public void Should_remove_memo_with_provided_contest_ID_value()
        {
            // Arrange
            Country sut = InitializeCountry();

            var (contestId1Of2, contestId2Of2) = ContestIds.GetTwo();

            sut.AddMemo(contestId1Of2);
            sut.AddMemo(contestId2Of2);

            // Assert
            Assert.Equivalent((ContestMemo[])
            [
                new ContestMemo(contestId1Of2, ContestStatus.Initialized),
                new ContestMemo(contestId2Of2, ContestStatus.Initialized)
            ], sut.ContestMemos);

            // Act
            sut.RemoveMemo(contestId2Of2);

            // Assert
            ContestMemo remainingMemo = Assert.Single(sut.ContestMemos);
            Assert.Equal(new ContestMemo(contestId1Of2, ContestStatus.Initialized), remainingMemo);
        }

        [Fact]
        public void Should_throw_when_has_no_memo_with_provided_contest_ID_value()
        {
            // Arrange
            Country sut = InitializeCountry();

            ContestId contestId = ContestIds.GetOne();

            // Assert
            Assert.Empty(sut.ContestMemos);

            // Act
            Action act = () => sut.RemoveMemo(contestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemos collection contains no item with the provided ContestId value.", exception.Message);

            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = InitializeCountry();

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

            (bool isError, Country result) = (errorsOrResult.IsError, errorsOrResult.Value);

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
