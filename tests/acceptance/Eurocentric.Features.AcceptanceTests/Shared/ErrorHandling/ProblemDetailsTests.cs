using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class ProblemDetailsTests : AcceptanceTestBase
{
    public ProblemDetailsTests(WebAppFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_return_status_code_404_with_problem_details_given_request_for_non_existent_resource()
    {
        // Arrange
        const string route = "/admin/api/v1.0/contests/4f5d7214-3fd9-43ec-8974-4b91c22ba15b";

        RestRequest request = new(route);

        request.UseSecretApiKey();

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        Assert.Equal("GET /admin/api/v1.0/contests/4f5d7214-3fd9-43ec-8974-4b91c22ba15b", problemDetails.Instance);
    }

    [Fact]
    public async Task Should_return_status_code_422_with_problem_details_given_request_that_conflicts_with_system_state()
    {
        // Arrange
        const string route = "/admin/api/v1.0/contests";

        const string json = """
                            {
                                "contestYear": 2022,
                                "cityName": "Turin",
                                "contestFormat": "Stockholm",
                                "group1Participants": [
                                    {
                                        "countryId": "83fa5568-f3be-4491-86e3-f442a792883c",
                                        "actName": "A",
                                        "songTitle": "S"
                                    },
                                    {
                                        "countryId": "1defc648-ca84-47ad-a5be-b0d47ef4cb1c",
                                        "actName": "A",
                                        "songTitle": "S"
                                    },
                                    {
                                        "countryId": "3b3005a4-b396-4dbd-955d-333e012bf8eb",
                                        "actName": "A",
                                        "songTitle": "S"
                                    }
                                ],
                                "group2Participants": [
                                    {
                                        "countryId": "3796c847-3dc8-4896-8eb0-0d5ca01db989",
                                        "actName": "A",
                                        "songTitle": "S"
                                    },
                                    {
                                        "countryId": "7a21c28a-0be1-45e4-914c-39e3d08cff90",
                                        "actName": "A",
                                        "songTitle": "S"
                                    },
                                    {
                                        "countryId": "5bb3252d-bb04-4ef4-b15c-f1dba527ee8a",
                                        "actName": "A",
                                        "songTitle": "S"
                                    }
                                ]
                            }
                            """;

        RestRequest request = new(route, Method.Post);

        request.UseSecretApiKey()
            .AddJsonBody(json);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status409Conflict, problemDetails.Status);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
    }

    [Fact]
    public async Task Should_return_status_code_422_with_problem_details_given_intrinsically_illegal_request()
    {
        // Arrange
        const string route = "/admin/api/v1.0/contests";
        const string json = """
                            {
                                "contestYear": 2022,
                                "cityName": "Turin",
                                "contestFormat": "Stockholm",
                                "group1Participants": [],
                                "group2Participants": []
                            }
                            """;

        RestRequest request = new(route, Method.Post);

        request.UseSecretApiKey()
            .AddJsonBody(json);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status422UnprocessableEntity, problemDetails.Status);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
    }
}
