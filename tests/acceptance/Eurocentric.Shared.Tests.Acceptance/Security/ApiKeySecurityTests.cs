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
        private const string Route = Apis.Admin.V0.Latest.Uri + "contests";
        private static readonly CreateContestRequest DummyRequest = new()
        {
            ContestYear = 2025, HostCityName = "Basel", VotingRules = VotingRules.Liverpool
        };

        public AdminApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_Admin_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = PostRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("X-Api-Key", TestApiKeys.Admin)
                .AddJsonBody(DummyRequest);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_authenticate_but_not_authorize_request_with_Public_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = PostRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("X-Api-Key", TestApiKeys.Public)
                .AddJsonBody(DummyRequest);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_not_authenticate_request_with_unrecognized_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = PostRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("X-Api-Key", TestApiKeys.Unrecognized)
                .AddJsonBody(DummyRequest);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_not_authenticate_request_without_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = PostRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(DummyRequest);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }

    public sealed class PublicApi : SeededWebAppTests
    {
        private const string Route = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_Admin_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = GetRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddQueryParameter("targetCountryCode", "GB")
                .AddHeader("X-Api-Key", TestApiKeys.Admin);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_Public_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = GetRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddQueryParameter("targetCountryCode", "GB")
                .AddHeader("X-Api-Key", TestApiKeys.Public);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_not_authenticate_request_with_unrecognized_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = GetRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddQueryParameter("targetCountryCode", "GB")
                .AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_not_authenticate_request_without_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = GetRequest.To(Route)
                .AddHeader("Accept", "application/json")
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
