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
        private const string ResourceUri = Apis.Admin.V0.Latest.Uri + "contests";
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
            RestRequest request = RestRequestFactory.Post(ResourceUri)
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
            RestRequest request = RestRequestFactory.Post(ResourceUri)
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
            RestRequest request = RestRequestFactory.Post(ResourceUri)
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
            RestRequest request = RestRequestFactory.Post(ResourceUri)
                .AddJsonBody(DummyRequest);

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }

    public sealed class PublicApi : SeededWebAppTests
    {
        private const string ResourceUri = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_Admin_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = RestRequestFactory.Get(ResourceUri)
                .AddHeader("X-Api-Key", TestApiKeys.Admin)
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_authenticate_and_authorize_request_with_Public_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = RestRequestFactory.Get(ResourceUri)
                .AddHeader("X-Api-Key", TestApiKeys.Public)
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_not_authenticate_request_with_unrecognized_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = RestRequestFactory.Get(ResourceUri)
                .AddHeader("X-Api-Key", TestApiKeys.Unrecognized)
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_not_authenticate_request_without_API_key_as_request_header()
        {
            // Arrange
            RestRequest request = RestRequestFactory.Get(ResourceUri)
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
