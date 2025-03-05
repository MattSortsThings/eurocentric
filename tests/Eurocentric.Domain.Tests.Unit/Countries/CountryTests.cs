using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Tests.Unit.Utils;
using Eurocentric.Domain.Tests.Unit.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Tests.Unit.Countries;

public static class CountryTests
{
    private static readonly DateTimeOffset FixedDateTimeOffset = DateTimeOffset.Parse("01/01/2025 12:30:00");

    private static void ShouldHaveCountryCodeValue(this Country subject, string expectedValue) =>
        Assert.Equal(expectedValue, subject.CountryCode.Value);

    private static void ShouldHaveCountryNameValue(this Country subject, string expectedValue) =>
        Assert.Equal(expectedValue, subject.CountryName.Value);

    private static void ShouldHaveEmptyCountryIdsCollection(this Country subject) => Assert.Empty(subject.ContestIds);

    private static void ShouldHaveCountryType(this Country subject, CountryType expectedValue) =>
        Assert.Equal(expectedValue, subject.CountryType);

    public sealed class AddContestIdMethod : UnitTest
    {
        private static readonly Guid FixedContestId = Guid.Parse("2a0fc598-a1ba-4976-a5af-33ac8c56f72e");

        private static Country CreateCountry() => Country.CreateReal()
            .WithCountryCode("GB")
            .AndCountryName("United Kingdom")
            .Build(FixedDateTimeOffset).Value;

        [Fact]
        public void Should_add_contestId_arg()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(FixedContestId);

            // Assert
            sut.ContestIds.ShouldNotContain(contestId);

            // Act
            sut.AddContestId(contestId);

            // Assert
            sut.ContestIds.ShouldContainSingle(contestId);
        }

        [Fact]
        public void Should_throw_when_ContestId_matching_contestId_arg_is_already_present()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(FixedContestId);

            sut.AddContestId(contestId);

            // Assert
            sut.ContestIds.ShouldContainSingle(contestId);

            // Act
            Action action = () => sut.AddContestId(contestId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(action);
            exception.ShouldHaveMessage($"ContestId {contestId.Value} is already present in country {sut.Id.Value}.");
        }

        [Fact]
        public void Should_throw_when_contestId_arg_is_null()
        {
            // Arrange
            Country sut = CreateCountry();

            // Act
            Action action = () => sut.AddContestId(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(action);
            exception.ShouldHaveMessage("Value cannot be null. (Parameter 'contestId')");
        }
    }

    public sealed class RemoveContestIdMethod : UnitTest
    {
        private static readonly Guid FixedContestId = Guid.Parse("2a0fc598-a1ba-4976-a5af-33ac8c56f72e");

        private static Country CreateCountry() => Country.CreateReal()
            .WithCountryCode("GB")
            .AndCountryName("United Kingdom")
            .Build(FixedDateTimeOffset).Value;

        [Fact]
        public void Should_remove_ContestId_matching_contestId_arg()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(FixedContestId);

            sut.AddContestId(contestId);

            // Assert
            sut.ContestIds.ShouldContainSingle(contestId);

            // Act
            sut.RemoveContestId(contestId);

            // Assert
            sut.ContestIds.ShouldNotContain(contestId);
        }

        [Fact]
        public void Should_throw_when_no_ContestId_matching_contestId_arg_is_present()
        {
            // Arrange
            Country sut = CreateCountry();

            ContestId contestId = ContestId.FromValue(FixedContestId);

            // Assert
            sut.ContestIds.ShouldNotContain(contestId);

            // Act
            Action action = () => sut.RemoveContestId(contestId);

            // Assert
            ArgumentException exception = action.ShouldThrow<ArgumentException>();
            exception.ShouldHaveMessage($"ContestId {contestId.Value} is not present in country {sut.Id.Value}.");
        }

        [Fact]
        public void Should_throw_when_contestId_arg_is_null()
        {
            // Arrange
            Country sut = CreateCountry();

            // Act
            Action action = () => sut.RemoveContestId(null!);

            // Assert
            ArgumentNullException exception = action.ShouldThrow<ArgumentNullException>();
            exception.ShouldHaveMessage("Value cannot be null. (Parameter 'contestId')");
        }
    }

    public sealed class FluentBuilder : UnitTest
    {
        private const string CountryCode = "GB";
        private const string CountryName = "United Kingdom";

        [Fact]
        public void Should_create_valid_Real_Country_given_valid_args()
        {
            // Act
            (bool isError, Country result) = Country.CreateReal()
                .WithCountryCode(CountryCode)
                .AndCountryName(CountryName)
                .Build(FixedDateTimeOffset)
                .ParseAsSuccess();

            // Assert
            isError.ShouldBeFalse();

            result.ShouldHaveCountryCodeValue(CountryCode);
            result.ShouldHaveCountryNameValue(CountryName);
            result.ShouldHaveCountryType(CountryType.Real);
            result.ShouldHaveEmptyCountryIdsCollection();
        }

        [Fact]
        public void Should_create_valid_Pseudo_Country_given_valid_args()
        {
            // Act
            (bool isError, Country result) = Country.CreatePseudo()
                .WithCountryCode(CountryCode)
                .AndCountryName(CountryName)
                .Build(FixedDateTimeOffset)
                .ParseAsSuccess();

            // Assert
            isError.ShouldBeFalse();

            result.ShouldHaveCountryCodeValue(CountryCode);
            result.ShouldHaveCountryNameValue(CountryName);
            result.ShouldHaveCountryType(CountryType.Pseudo);
            result.ShouldHaveEmptyCountryIdsCollection();
        }

        [Fact]
        public void Should_return_errors_when_countryCode_arg_is_invalid()
        {
            // Arrange
            const string invalidCountryCode = "*";

            // Act
            (bool isError, Error firstError) = Country.CreateReal()
                .WithCountryCode(invalidCountryCode)
                .AndCountryName(CountryName)
                .Build(FixedDateTimeOffset)
                .ParseAsError();

            // Assert
            isError.ShouldBeTrue();
            firstError.ShouldHaveFailureErrorType();
            firstError.ShouldHaveCode("Invalid country code");
        }

        [Fact]
        public void Should_return_errors_when_countryName_arg_is_invalid()
        {
            // Arrange
            const string invalidCountryName = "";

            // Act
            (bool isError, Error firstError) = Country.CreateReal()
                .WithCountryCode(CountryCode)
                .AndCountryName(invalidCountryName)
                .Build(FixedDateTimeOffset)
                .ParseAsError();

            // Assert
            isError.ShouldBeTrue();
            firstError.ShouldHaveFailureErrorType();
            firstError.ShouldHaveCode("Invalid country name");
        }
    }
}
