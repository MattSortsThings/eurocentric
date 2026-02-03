using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Placeholders;
using Eurocentric.Tests.Acceptance.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Placeholders;

public static class GetBlobbyTests
{
    [Category("placeholder")]
    [NotInParallel(ConstraintKey)]
    public sealed class Endpoint : AcceptanceTests
    {
        private const string ConstraintKey = "PublicApi.V0.Placeholders.GetBlobbyTests.Endpoint";

        [Test]
        [MethodDataSource(nameof(HappyPathTestCases))]
        public async Task Should_succeed_with_200_OK_and_requested_Blobbies(int count)
        {
            // Arrange
            RestRequest request = new RestRequest("/public/api/v0/blobbies").AddQueryParameter("count", count);

            // Act
            Result<RestResponse<GetBlobbiesResponseBody>, RestResponse<ProblemDetails>> response =
                await SystemUnderTest.SendRequestAsync<GetBlobbiesResponseBody>(request);

            // Assert
            await Assert.That(response.IsSuccess).IsTrue();

            await Assert
                .That(response.Value.Data?.Blobbies)
                .Count()
                .IsEqualTo(count)
                .And.ContainsOnly(stringValue => stringValue == "Blobby");
        }

        public static IEnumerable<int> HappyPathTestCases() => Enumerable.Range(1, 25);
    }
}
