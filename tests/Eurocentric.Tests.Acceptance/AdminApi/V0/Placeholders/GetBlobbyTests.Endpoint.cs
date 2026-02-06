using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Placeholders;
using Eurocentric.Tests.Acceptance.AdminApi.V0.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.AdminApi.V0.Placeholders;

public sealed class GetBlobbyTests_Endpoint : AdminApiV0AcceptanceTests
{
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
