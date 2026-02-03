using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Placeholders;
using Eurocentric.Tests.Acceptance.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.AdminApi.V0.Placeholders;

public static class GetBlobbyTests
{
    [Category("placeholder")]
    [NotInParallel(ConstraintKey)]
    public sealed class Endpoint : AcceptanceTests
    {
        private const string ConstraintKey = "AdminApi.V0.Placeholders.GetBlobbyTests.Endpoint";

        [Test]
        [Repeat(4)]
        public async Task Should_succeed_with_200_OK_and_fixed_Blobbies()
        {
            // Arrange
            RestRequest request = new("/admin/api/v0/blobbies");

            // Act
            Result<RestResponse<GetBlobbiesResponseBody>, RestResponse<ProblemDetails>> response =
                await SystemUnderTest.SendRequestAsync<GetBlobbiesResponseBody>(request);

            // Assert
            await Assert.That(response.IsSuccess).IsTrue();

            await Assert
                .That(response.Value.Data?.Blobbies)
                .Count()
                .IsEqualTo(3)
                .And.ContainsOnly(stringValue => stringValue == "Blobby");
        }
    }
}
