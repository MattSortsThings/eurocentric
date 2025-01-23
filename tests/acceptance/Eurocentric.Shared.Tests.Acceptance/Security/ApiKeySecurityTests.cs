using System.Net;
using Eurocentric.AdminApi.V0.Contests.CreateContest;
using Eurocentric.AdminApi.V0.Contests.Models;
using Eurocentric.Shared.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.Shared.Tests.Acceptance.Security;

public static class ApiKeySecurityTests
{
    public sealed class AdminApi : SeededWebAppTests
    {
        private static readonly CreateContestRequest DummyRequest = new()
        {
            ContestYear = 2025, HostCityName = "Basel", VotingRules = VotingRules.Liverpool
        };

        public AdminApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_admin_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Post.To(UriSegments.AdminApi.V0Latest + "contests")
                .AddJsonBody(DummyRequest)
                .AddHeader("X-Api-Key", TestApiKeys.Admin);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_authenticate_but_not_authorize_request_with_public_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Post.To(UriSegments.AdminApi.V0Latest + "contests")
                .AddJsonBody(DummyRequest)
                .AddHeader("X-Api-Key", TestApiKeys.Public);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_not_authenticate_request_with_unrecognized_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Post.To(UriSegments.AdminApi.V0Latest + "contests")
                .AddJsonBody(DummyRequest)
                .AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_not_authenticate_request_without_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Post.To(UriSegments.AdminApi.V0Latest + "contests")
                .AddJsonBody(DummyRequest);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }

    public sealed class PublicApi : SeededWebAppTests
    {
        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_admin_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To(UriSegments.PublicApi.V0Latest + "voting-country-rankings/points-share")
                .AddQueryParameter("targetCountryCode", "GB")
                .AddHeader("X-Api-Key", TestApiKeys.Admin);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_public_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To(UriSegments.PublicApi.V0Latest + "voting-country-rankings/points-share")
                .AddQueryParameter("targetCountryCode", "GB")
                .AddHeader("X-Api-Key", TestApiKeys.Public);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_not_authenticate_request_with_unrecognized_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To(UriSegments.PublicApi.V0Latest + "voting-country-rankings/points-share")
                .AddQueryParameter("targetCountryCode", "GB")
                .AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_not_authenticate_request_without_api_key_as_request_header()
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To(UriSegments.PublicApi.V0Latest + "voting-country-rankings/points-share")
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}
