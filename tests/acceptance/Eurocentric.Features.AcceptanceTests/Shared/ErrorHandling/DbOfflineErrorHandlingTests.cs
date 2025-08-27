using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class DbOfflineErrorHandlingTests : ParallelDbOfflineAcceptanceTest
{
    [Test]
    public async Task Endpoint_using_EF_Core_should_return_503_with_ProblemDetails_when_database_offline()
    {
        // Arrange
        const string route = "/admin/api/v1.0/countries";

        RestRequest request = new RestRequest(route).UseSecretApiKey();

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.ServiceUnavailable);

        await Assert.That(problem.Headers).Contains(header => header is { Name: "Retry-After", Value: "120" });

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(503)
            .And.HasTitle("Database timeout")
            .And.HasDetail("SqlException was thrown while handling the request because the database connection " +
                           "or operation timed out. Please retry after c.120 seconds.")
            .And.HasInstance("GET /admin/api/v1.0/countries")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.6.4");
    }

    [Test]
    public async Task Endpoint_using_Dapper_should_return_503_with_ProblemDetails_when_database_offline()
    {
        // Arrange
        const string route = "/public/api/v1.0/rankings/competing-countries/points-average";

        RestRequest request = new RestRequest(route).UseSecretApiKey();

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.ServiceUnavailable);

        await Assert.That(problem.Headers).Contains(header => header is { Name: "Retry-After", Value: "120" });

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(503)
            .And.HasTitle("Database timeout")
            .And.HasDetail("SqlException was thrown while handling the request because the database connection " +
                           "or operation timed out. Please retry after c.120 seconds.")
            .And.HasInstance("GET /public/api/v1.0/rankings/competing-countries/points-average")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.6.4");
    }
}
