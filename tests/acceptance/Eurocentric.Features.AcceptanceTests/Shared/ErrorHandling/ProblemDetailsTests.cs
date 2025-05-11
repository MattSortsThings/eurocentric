using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class ProblemDetailsTests : AcceptanceTestBase
{
    public ProblemDetailsTests(WebAppFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_return_status_code_422_with_problem_details_given_intrinsically_illegal_request()
    {
        // Arrange
        const int illegalContestYear = 0;

        var requestBody = new
        {
            ContestYear = illegalContestYear, CityName = "CityName", ContestFormat = ContestFormat.Stockholm
        };

        RestRequest request = new("admin/api/v0.2/contests", Method.Post);

        request.AddJsonBody(requestBody);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status422UnprocessableEntity, problemDetails.Status);
        Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
    }
}
