using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Countries;

public sealed class CountryTests : UnitTestBase
{
    private static Country CreateCountryWithDefaultValues() => Country.Create()
        .WithCountryCode(CountryCode.FromValue("AA"))
        .WithName(CountryName.FromValue("CountryName"))
        .Build(() => CountryId.FromValue(Guid.Parse("2963534f-85b8-4546-a240-b82631898035")))
        .Value;

    public sealed class AddMemoMethod : UnitTestBase
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("785913e9-dad3-4504-90aa-34e1175fb02e"));

        [Fact]
        public void Should_add_contest_memo_with_initialized_status()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();

            // Assert
            Assert.Empty(sut.ContestMemos);

            // Act
            sut.AddMemo(FixedContestId);

            // Assert
            Assert.Single(sut.ContestMemos, new ContestMemo(FixedContestId, ContestStatus.Initialized));
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();

            // Act
            Action act = () => sut.AddMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);

            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_when_existing_contest_memo_has_provided_contest_ID()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();
            sut.AddMemo(FixedContestId);

            // Assert
            Assert.Single(sut.ContestMemos);

            // Act
            Action act = () => sut.AddMemo(FixedContestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemos collection contains an item with the provided ContestId value.", exception.Message);

            Assert.Single(sut.ContestMemos);
        }
    }

    public sealed class ReplaceMemoMethod : UnitTestBase
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("785913e9-dad3-4504-90aa-34e1175fb02e"));

        [Fact]
        public void Should_replace_contest_memo_with_new_memo_having_provided_status()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();
            sut.AddMemo(FixedContestId);

            // Assert
            Assert.Single(sut.ContestMemos, new ContestMemo(FixedContestId, ContestStatus.Initialized));

            // Act
            sut.ReplaceMemo(FixedContestId, ContestStatus.InProgress);

            // Assert
            Assert.Single(sut.ContestMemos, new ContestMemo(FixedContestId, ContestStatus.InProgress));
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();

            // Act
            Action act = () => sut.ReplaceMemo(null!, ContestStatus.InProgress);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);

            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_when_no_existing_contest_memo_has_provided_contest_ID()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();

            // Act
            Action act = () => sut.ReplaceMemo(FixedContestId, ContestStatus.InProgress);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemos collection contains no item with the provided ContestId value.", exception.Message);

            Assert.Empty(sut.ContestMemos);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("785913e9-dad3-4504-90aa-34e1175fb02e"));

        [Fact]
        public void Should_remove_contest_memo_having_provided_contest_ID()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();
            sut.AddMemo(FixedContestId);

            // Assert
            Assert.Single(sut.ContestMemos, new ContestMemo(FixedContestId, ContestStatus.Initialized));

            // Act
            sut.RemoveMemo(FixedContestId);

            // Assert
            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();

            // Act
            Action act = () => sut.RemoveMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);

            Assert.Empty(sut.ContestMemos);
        }

        [Fact]
        public void Should_throw_when_no_existing_contest_memo_has_provided_contest_ID()
        {
            // Arrange
            Country sut = CreateCountryWithDefaultValues();

            // Act
            Action act = () => sut.RemoveMemo(FixedContestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ContestMemos collection contains no item with the provided ContestId value.", exception.Message);

            Assert.Empty(sut.ContestMemos);
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
        public void Should_throw_given_null_IDProvider_arg()
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
