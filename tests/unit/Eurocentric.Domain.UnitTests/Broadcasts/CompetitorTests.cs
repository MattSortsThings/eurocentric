using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Broadcasts;

public sealed class CompetitorTests : UnitTestBase
{
    public sealed class BroadcastCompetitorComparer : UnitTestBase
    {
        private static Competitor CreateCompetitor(CountryId countryId,
            PointsValue[]? juryPoints = null,
            PointsValue[]? televotePoints = null,
            int runningOrderPosition = 0)
        {
            Competitor competitor = new(countryId, runningOrderPosition);

            foreach (PointsValue p in juryPoints ?? [])
            {
                competitor.ReceiveAward(new JuryAward(CountryId.FromValue(Guid.NewGuid()), p));
            }

            foreach (PointsValue p in televotePoints ?? [])
            {
                competitor.ReceiveAward(new TelevoteAward(CountryId.FromValue(Guid.NewGuid()), p));
            }

            return competitor;
        }

        [Fact]
        public void Should_return_zero_when_x_and_y_are_same_instance()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitor = CreateCompetitor(TestCountryIds.At, runningOrderPosition: 1);

            // Act
            int result = sut.Compare(competitor, competitor);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_return_positive_value_when_y_arg_is_null()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitor = CreateCompetitor(TestCountryIds.At, runningOrderPosition: 1);

            // Act
            int result = sut.Compare(competitor, null);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_return_negative_value_when_x_arg_is_null()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitor = CreateCompetitor(TestCountryIds.At, runningOrderPosition: 1);

            // Act
            int result = sut.Compare(null, competitor);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Should_return_zero_when_x_and_y_have_empty_points_awards_and_equal_RunningOrderPosition_values()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [],
                juryPoints: []);

            Competitor competitorB = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [],
                juryPoints: []);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_compare_by_total_points()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.One],
                juryPoints: [PointsValue.Ten]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Ten],
                juryPoints: [PointsValue.Zero]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_on_total_points_tie()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Two, PointsValue.Zero],
                juryPoints: [PointsValue.One, PointsValue.Zero]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.One, PointsValue.Zero],
                juryPoints: [PointsValue.One, PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Should_compare_by_non_zero_points_televote_awards_on_televote_points_tie()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.One, PointsValue.One],
                juryPoints: [PointsValue.One, PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Two, PointsValue.Zero],
                juryPoints: [PointsValue.One, PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_1()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Ten, PointsValue.Four],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Twelve, PointsValue.Two],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_2()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Twelve, PointsValue.Six, PointsValue.Five],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Twelve, PointsValue.Ten, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_3()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Ten, PointsValue.Six, PointsValue.Three],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Ten, PointsValue.Eight, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_4()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Eight, PointsValue.Five, PointsValue.Three],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Eight, PointsValue.Seven, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_5()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Seven, PointsValue.Five, PointsValue.Two],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Seven, PointsValue.Six, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_6()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Six, PointsValue.Four, PointsValue.Two],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Six, PointsValue.Five, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_7()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Five, PointsValue.Three, PointsValue.Two],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Five, PointsValue.Four, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_televote_points_value_countback_on_non_zero_points_televote_awards_tie_scenario_8()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Four, PointsValue.Two, PointsValue.Two],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Four, PointsValue.Three, PointsValue.One],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Should_compare_by_running_order_position_on_televote_points_value_countback_tie()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [PointsValue.Three, PointsValue.Two, PointsValue.One, PointsValue.Zero],
                juryPoints: [PointsValue.One]);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 2,
                televotePoints: [PointsValue.Three, PointsValue.Two, PointsValue.One, PointsValue.Zero],
                juryPoints: [PointsValue.One]);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Should_compare_by_RunningOrderPosition_when_both_have_empty_points_awards()
        {
            // Arrange
            IComparer<Competitor> sut = Competitor.GetBroadcastCompetitorComparer();

            Competitor competitorA = CreateCompetitor(TestCountryIds.At,
                runningOrderPosition: 1,
                televotePoints: [],
                juryPoints: []);

            Competitor competitorB = CreateCompetitor(TestCountryIds.Be,
                runningOrderPosition: 2,
                televotePoints: [],
                juryPoints: []);

            // Act
            int result = sut.Compare(competitorA, competitorB);

            // Assert
            Assert.True(result < 0);
        }
    }
}
