using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.ErrorHandling;

[Category("error-handling")]
public sealed class ProblemDetailsResponseTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Category("clear-box")]
    public async Task Should_return_404_with_problem_details_on_not_found_domain_error()
    {
        // Arrange
        Guid nonExistentCountryId = Guid.Parse("01234567-abcd-abcd-abcd-000000000000");

        RestRequest getCountryRequest = GetRequest("/admin/api/v0.1/countries/{countryId}")
            .AddUrlSegment("countryId", nonExistentCountryId);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getCountryRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

        await Assert
            .That(problem.Data)
            .HasTitle("Country not found")
            .And.HasDetail("The requested country does not exist.")
            .And.HasStatus(StatusCodes.Status404NotFound)
            .And.HasInstance("GET /admin/api/v0.1/countries/01234567-abcd-abcd-abcd-000000000000")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.5")
            .And.HasExtension("countryId", nonExistentCountryId);
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_409_with_problem_details_on_conflict_domain_error()
    {
        // Guid
        Guid countryWithContestRolesId = Guid.Parse("01979615-1e4c-7ba1-868b-018ce12e1c0c");

        RestRequest deleteCountryRequest = DeleteRequest("/admin/api/v0.1/countries/{countryId}")
            .AddUrlSegment("countryId", countryWithContestRolesId);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            deleteCountryRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Conflict);

        await Assert
            .That(problem.Data)
            .HasTitle("Country deletion not allowed")
            .And.HasDetail("The requested country has a role in one or more contests.")
            .And.HasStatus(StatusCodes.Status409Conflict)
            .And.HasInstance("DELETE /admin/api/v0.1/countries/01979615-1e4c-7ba1-868b-018ce12e1c0c")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.10")
            .And.HasExtension("countryId", countryWithContestRolesId);
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_422_with_problem_details_on_unprocessable_domain_error()
    {
        // Arrange
        const int illegalPageSizeValue = 999999;

        RestRequest getRankingsRequest = GetRequest("/public/api/v0.2/competing-country-rankings/points-average")
            .AddQueryParameter("pageSize", illegalPageSizeValue);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getRankingsRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);

        await Assert
            .That(problem.Data)
            .HasTitle("Illegal page size value")
            .And.HasDetail("Page size value must be an integer between 1 and 100.")
            .And.HasStatus(StatusCodes.Status422UnprocessableEntity)
            .And.HasInstance("GET /public/api/v0.2/competing-country-rankings/points-average?pageSize=999999")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.21")
            .And.HasExtension("pageSize", illegalPageSizeValue);
    }

    private static RestRequest GetRequest(string route) => new(route);

    private static RestRequest DeleteRequest(string route) => new(route, Method.Delete);
}
