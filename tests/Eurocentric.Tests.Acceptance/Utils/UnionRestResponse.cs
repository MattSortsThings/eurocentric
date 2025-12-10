using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     The discriminated union of <i>either</i> a successful REST response <i>or</i> an unsuccessful REST response with
///     a deserialized <see cref="ProblemDetails" /> response body object.
/// </summary>
public sealed class UnionRestResponse
{
    private readonly RestResponse? _successfulResponse;
    private readonly RestResponse<ProblemDetails>? _unsuccessfulResponse;

    private UnionRestResponse(
        RestResponse? successfulResponse = null,
        RestResponse<ProblemDetails>? unsuccessfulResponse = null
    )
    {
        _successfulResponse = successfulResponse;
        _unsuccessfulResponse = unsuccessfulResponse;
    }

    /// <summary>
    ///     Gets a boolean value indicating whether the underlying REST response is successful.
    /// </summary>
    public bool IsSuccessful => _successfulResponse is not null;

    /// <summary>
    ///     Gets the underlying successful REST response.
    /// </summary>
    /// <returns>A <see cref="RestResponse" /> instance with a successful status code.</returns>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse AsSuccessful() =>
        _successfulResponse ?? throw new InvalidOperationException("Response is unsuccessful.");

    /// <summary>
    ///     Gets the underlying unsuccessful REST response.
    /// </summary>
    /// <returns>
    ///     A <see cref="RestResponse{T}" /> instance with an unsuccessful status code and a deserialized
    ///     <see cref="ProblemDetails" /> response body object.
    /// </returns>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsUnsuccessful() =>
        _unsuccessfulResponse ?? throw new InvalidOperationException("Response is successful.");

    public static implicit operator UnionRestResponse(RestResponse response) => new(response);

    public static implicit operator UnionRestResponse(RestResponse<ProblemDetails> response) =>
        new(unsuccessfulResponse: response);
}
