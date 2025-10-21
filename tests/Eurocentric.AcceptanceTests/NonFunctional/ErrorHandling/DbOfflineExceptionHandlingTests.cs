using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.ErrorHandling;

[Category("error-handling")]
public sealed class DbOfflineExceptionHandlingTests : SerialCleanAcceptanceTest
{
    [Test]
    [Category("clear-box")]
    public async Task Should_return_503_with_problem_details_on_DB_timeout_using_Dapper()
    {
        // Arrange
        await SystemUnderTest.ExecuteScopedAsync(BackDoorOperations.PauseDbAsync);

        RestRequest getRankingsRequest = GetRequest("/public/api/v0.2/competing-country-rankings/points-average")
            .UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getRankingsRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.ServiceUnavailable);

        await Assert.That(problem).HasHeader("Retry-After", "120");

        await Assert
            .That(problem.Data)
            .IsNotNull()
            .And.HasTitle("Database timeout")
            .And.HasDetail(
                "SqlException due to database timeout was thrown while handling the request. "
                    + "Please retry after the interval specified in the \"Retry-After\" header."
            )
            .And.HasStatus(StatusCodes.Status503ServiceUnavailable)
            .And.HasInstance("GET /public/api/v0.2/competing-country-rankings/points-average")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.6.4");
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_503_with_problem_details_on_DB_timeout_using_EF_Core()
    {
        // Arrange
        await SystemUnderTest.ExecuteScopedAsync(BackDoorOperations.PauseDbAsync);

        RestRequest getCountriesRequest = GetRequest("/admin/api/v0.2/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getCountriesRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem).HasHeader("Retry-After", "120");

        await Assert
            .That(problem.Data)
            .IsNotNull()
            .And.HasTitle("Database timeout")
            .And.HasDetail(
                "SqlException due to database timeout was thrown while handling the request. "
                    + "Please retry after the interval specified in the \"Retry-After\" header."
            )
            .And.HasStatus(StatusCodes.Status503ServiceUnavailable)
            .And.HasInstance("GET /admin/api/v0.2/countries")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.6.4");
    }

    private static RestRequest GetRequest(string route) => new(route);
}
