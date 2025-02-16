using ErrorOr;
using Eurocentric.PublicApi.Tests.Integration.Utils;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Eurocentric.PublicApi.V0.Greetings.Models;

namespace Eurocentric.PublicApi.Tests.Integration.V0.Greetings;

public static class GetGreetingsTests
{
    public sealed class Handler : IntegrationTest
    {
        public Handler(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData(2, Language.English, "Agnetha", "Hi Agnetha!", "Hi Agnetha!")]
        [InlineData(2, Language.French, "Bjorn", "Bonjour Bjorn!", "Bonjour Bjorn!")]
        [InlineData(1, Language.Dutch, "Benny", "Hoi Benny!")]
        [InlineData(1, Language.Swedish, "Anni-Frid", "Hej Anni-Frid!")]
        public async Task Should_return_result_with_greetings_given_valid_query(int quantity,
            Language language,
            string clientName,
            params string[] greetings)
        {
            // Arrange
            GetGreetingsQuery query = new() { Quantity = quantity, Language = language, ClientName = clientName };

            GetGreetingsResult expectedResult = new(greetings);

            // Act
            ErrorOr<GetGreetingsResult> errorOrResult = await SendAsync(query);

            // Assert
            Assert.Multiple(
                () => Assert.False(errorOrResult.IsError),
                () => Assert.Equivalent(expectedResult, errorOrResult.Value)
            );
        }
    }
}
