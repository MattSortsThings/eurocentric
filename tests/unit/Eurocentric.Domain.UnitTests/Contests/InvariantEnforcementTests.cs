using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class InvariantEnforcementTests : UnitTestBase
{
    public sealed class FailOnContestYearConflictExtensionMethod : UnitTestBase
    {
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;

        [Fact]
        public void Should_return_contest_when_existingContests_arg_is_empty_list()
        {
            // Arrange
            (Country at, Country be, Country cz, Country dk, Country ee, Country fi) =
                (TestCountries.At, TestCountries.Be, TestCountries.Cz, TestCountries.Dk, TestCountries.Ee, TestCountries.Fi);

            ContestId sutContestId = ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithArbitraryYearAndCity()
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(sutContestId));

            IQueryable<Contest> existingContests = Enumerable.Empty<Contest>().AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnContestYearConflict(existingContests);

            var (isError, outputContest) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.Same(sut.Value, outputContest);
        }

        [Fact]
        public void Should_return_contest_when_no_existing_contest_has_same_contest_year()
        {
            // Arrange
            const int sutContestYearValue = 2025;
            const int otherContestYearValue = 2016;

            (Country at, Country be, Country cz, Country dk, Country ee, Country fi) =
                (TestCountries.At, TestCountries.Be, TestCountries.Cz, TestCountries.Dk, TestCountries.Ee, TestCountries.Fi);

            ContestId sutContestId = ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));
            ContestId otherContestId = ContestId.FromValue(Guid.Parse("e9238edc-ad62-46db-af0e-aab3f5e737c8"));

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear.FromValue(sutContestYearValue))
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(sutContestId));

            Contest other = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear.FromValue(otherContestYearValue))
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(otherContestId))
                .Value;

            IQueryable<Contest> existingContests = new List<Contest> { other }.AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnContestYearConflict(existingContests);

            // Assert
            var (isError, outputContest) = (result.IsError, result.Value);

            Assert.False(isError);

            Assert.Same(sut.Value, outputContest);
        }

        [Fact]
        public void Should_return_errors_when_existing_contest_has_same_contest_year()
        {
            // Arrange
            const int sharedContestYearValue = 2025;

            (Country at, Country be, Country cz, Country dk, Country ee, Country fi) =
                (TestCountries.At, TestCountries.Be, TestCountries.Cz, TestCountries.Dk, TestCountries.Ee, TestCountries.Fi);

            ContestId sutContestId = ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));
            ContestId otherContestId = ContestId.FromValue(Guid.Parse("e9238edc-ad62-46db-af0e-aab3f5e737c8"));

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear.FromValue(sharedContestYearValue))
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(sutContestId));

            Contest other = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear.FromValue(sharedContestYearValue))
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(otherContestId))
                .Value;

            IQueryable<Contest> existingContests = new List<Contest> { other }.AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnContestYearConflict(existingContests);

            // Assert
            var (isError, outputContest, firstError) = (result.IsError, result.Value, result.FirstError);

            Assert.True(isError);

            Assert.Null(outputContest);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Contest year conflict", firstError.Code);
            Assert.Equal("A contest already exists with the provided contest year.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: sharedContestYearValue });
        }

        [Fact]
        public void Should_return_self_when_instance_is_errors()
        {
            // Arrange
            (Country at, Country be, Country cz) = (TestCountries.At, TestCountries.Be, TestCountries.Cz);

            ContestId sutContestId = ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithArbitraryYearAndCity()
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .Build(new FixedContestIdGenerator(sutContestId));

            IQueryable<Contest> dummyExistingContests = Enumerable.Empty<Contest>().AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnContestYearConflict(dummyExistingContests);

            // Assert
            Assert.Equal(sut, result);
        }
    }

    public sealed class FailOnOrphanParticipantExtensionMethod : UnitTestBase
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

        [Fact]
        public void Should_return_contest_when_every_participant_has_matching_country()
        {
            // Arrange
            (Country at, Country be, Country cz, Country dk, Country ee, Country fi) =
                (TestCountries.At, TestCountries.Be, TestCountries.Cz, TestCountries.Dk, TestCountries.Ee, TestCountries.Fi);

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithArbitraryYearAndCity()
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(FixedContestId));

            IQueryable<Country> existingCountries = new List<Country> { at, be, cz, dk, ee, fi }.AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnOrphanParticipant(existingCountries);

            var (isError, outputContest) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.Same(sut.Value, outputContest);
        }

        [Fact]
        public void Should_return_errors_when_participant_has_no_matching_country()
        {
            // Arrange
            (Country at, Country be, Country cz, Country dk, Country ee, Country fi) =
                (TestCountries.At, TestCountries.Be, TestCountries.Cz, TestCountries.Dk, TestCountries.Ee, TestCountries.Fi);

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithArbitraryYearAndCity()
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .WithGroup2Countries(dk.Id, ee.Id, fi.Id)
                .Build(new FixedContestIdGenerator(FixedContestId));

            IQueryable<Country> existingCountries = new List<Country> { at, be, cz, dk, ee }.AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnOrphanParticipant(existingCountries);

            var (isError, outputContest, firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(outputContest);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan participant", firstError.Code);
            Assert.Equal("No country exists with the provided participating country ID.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "participatingCountryId", Value: Guid g }
                                                        && g == fi.Id.Value);
        }

        [Fact]
        public void Should_return_self_when_instance_is_errors()
        {
            // Arrange
            (Country at, Country be, Country cz) = (TestCountries.At, TestCountries.Be, TestCountries.Cz);

            ErrorOr<Contest> sut = Contest.CreateStockholmFormat()
                .WithArbitraryYearAndCity()
                .WithGroup1Countries(at.Id, be.Id, cz.Id)
                .Build(new FixedContestIdGenerator(FixedContestId));

            IQueryable<Country> dummyExistingCountries = Array.Empty<Country>().AsQueryable();

            // Act
            ErrorOr<Contest> result = sut.FailOnOrphanParticipant(dummyExistingCountries);

            // Assert
            Assert.Equal(sut, result);
        }
    }
}
