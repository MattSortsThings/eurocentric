using ErrorOr;
using Eurocentric.AdminApi.Tests.Subcutaneous.Utils;
using Eurocentric.AdminApi.V0.Contests.CreateContest;
using Eurocentric.Domain.Entities.Contests;
using Eurocentric.Tests.Utils.Attributes;

namespace Eurocentric.AdminApi.Tests.Subcutaneous.V0.Contests;

public static class CreateContestTests
{
    private static void ShouldMatch(this Contest contest, CreateContestCommand command)
    {
        Assert.Equal(command.ContestYear, contest.ContestYear);
        Assert.Equal(command.HostCityName, contest.HostCityName);
        Assert.Equal(command.VotingRules, contest.VotingRules);
    }

    [PlaceholderTest]
    public sealed class Command : CleanWebAppTests
    {
        public Command(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_create_contest_and_return_it_given_valid_command()
        {
            // Arrange
            CreateContestCommand command = new(2025, "Basel", VotingRules.Liverpool);

            // Act
            ErrorOr<Contest> result = await Sut.Send(command, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => result.ShouldNotBeError(),
                () => result.Value.ShouldMatch(command)
            );
        }
    }
}
