using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils.Contracts;

/// <summary>
///     The discriminated union of <i>either</i> a success REST response with a raw response body <i>or</i> a failure
///     REST response with a deserialized <see cref="ProblemDetails" /> response body object.
/// </summary>
public sealed class SuccessOrFailureRestResponse
{
    private readonly RestResponse<ProblemDetails>? _failureResponse;
    private readonly RestResponse? _successResponse;

    private SuccessOrFailureRestResponse(
        RestResponse? successResponse = null,
        RestResponse<ProblemDetails>? failureResponse = null
    )
    {
        _successResponse = successResponse;
        _failureResponse = failureResponse;
    }

    /// <summary>
    ///     Gets a boolean value indicating whether the underlying REST response is a success.
    /// </summary>
    public bool IsSuccess => _successResponse is not null;

    /// <summary>
    ///     Gets the underlying success REST response instance.
    /// </summary>
    /// <returns>A success <see cref="RestResponse" /> object with a raw response body.</returns>
    /// <exception cref="InvalidOperationException">The underlying REST response is a failure.</exception>
    public RestResponse GetSuccessRestResponse() =>
        _successResponse ?? throw new InvalidOperationException("REST response is failure.");

    /// <summary>
    ///     Gets the underlying failure REST response instance.
    /// </summary>
    /// <returns>
    ///     A failure <see cref="RestResponse{T}" /> object with a deserialized <see cref="ProblemDetails" />
    ///     response body object.
    /// </returns>
    /// <exception cref="InvalidOperationException">The underlying REST response is a success.</exception>
    public RestResponse<ProblemDetails> GetFailureRestResponse() =>
        _failureResponse ?? throw new InvalidOperationException("REST response is success.");

    /// <summary>
    ///     Creates and returns a new <see cref="SuccessOrFailureRestResponse" /> that wraps the provided success REST
    ///     response.
    /// </summary>
    /// <param name="restResponse">The underlying success REST response for the instance to be created.</param>
    /// <returns>A new <see cref="RestResponse" /> instance.</returns>
    /// <exception cref="ArgumentException">The <paramref name="restResponse" /> argument is not successful.</exception>
    public static SuccessOrFailureRestResponse FromSuccessRestResponse(RestResponse restResponse) =>
        !restResponse.IsSuccessful
            ? throw new ArgumentException("REST response is not successful.")
            : new SuccessOrFailureRestResponse(failureResponse: null, successResponse: restResponse);

    /// <summary>
    ///     Creates and returns a new <see cref="SuccessOrFailureRestResponse" /> that wraps the provided failure REST
    ///     response.
    /// </summary>
    /// <param name="restResponse">The underlying failure REST response for the instance to be created.</param>
    /// <returns>A new <see cref="RestResponse" /> instance.</returns>
    /// <exception cref="ArgumentException">The <paramref name="restResponse" /> argument is successful.</exception>
    public static SuccessOrFailureRestResponse FromFailureRestResponse(RestResponse<ProblemDetails> restResponse) =>
        restResponse.IsSuccessful
            ? throw new ArgumentException("REST response is successful.")
            : new SuccessOrFailureRestResponse(null, restResponse);
}

/// <summary>
///     The discriminated union of <i>either</i> a success REST response with a deserialized response body object of type
///     <typeparamref name="TBody" /> <i>or</i> a failure REST response with a deserialized <see cref="ProblemDetails" />
///     response body object.
/// </summary>
/// <typeparam name="TBody">The successful response body type.</typeparam>
public sealed class SuccessOrFailureRestResponse<TBody>
    where TBody : class
{
    private readonly RestResponse<ProblemDetails>? _failureResponse;
    private readonly RestResponse<TBody>? _successResponse;

    private SuccessOrFailureRestResponse(
        RestResponse<TBody>? successResponse = null,
        RestResponse<ProblemDetails>? failureResponse = null
    )
    {
        _successResponse = successResponse;
        _failureResponse = failureResponse;
    }

    /// <summary>
    ///     Gets a boolean value indicating whether the underlying REST response is a success.
    /// </summary>
    public bool IsSuccess => _successResponse is not null;

    /// <summary>
    ///     Gets the underlying success REST response instance.
    /// </summary>
    /// <returns>
    ///     A success <see cref="RestResponse" /> object with a deserialized response body object of type
    ///     <typeparamref name="TBody" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">The underlying REST response is a failure.</exception>
    public RestResponse<TBody> GetSuccessRestResponse() =>
        _successResponse ?? throw new InvalidOperationException("REST response is failure.");

    /// <summary>
    ///     Gets the underlying failure REST response instance.
    /// </summary>
    /// <returns>
    ///     A failure <see cref="RestResponse{T}" /> object with a deserialized <see cref="ProblemDetails" />
    ///     response body object.
    /// </returns>
    /// <exception cref="InvalidOperationException">The underlying REST response is a success.</exception>
    public RestResponse<ProblemDetails> GetFailureRestResponse() =>
        _failureResponse ?? throw new InvalidOperationException("REST response is success.");

    /// <summary>
    ///     Creates and returns a new <see cref="SuccessOrFailureRestResponse{TBody}" /> that wraps the provided
    ///     success REST response.
    /// </summary>
    /// <param name="restResponse">The underlying success REST response for the instance to be created.</param>
    /// <returns>A new <see cref="RestResponse" /> instance.</returns>
    /// <exception cref="ArgumentException">The <paramref name="restResponse" /> argument is not successful.</exception>
    public static SuccessOrFailureRestResponse<TBody> FromSuccessRestResponse(RestResponse<TBody> restResponse) =>
        !restResponse.IsSuccessful
            ? throw new ArgumentException("REST response is not successful.")
            : new SuccessOrFailureRestResponse<TBody>(failureResponse: null, successResponse: restResponse);

    /// <summary>
    ///     Creates and returns a new <see cref="SuccessOrFailureRestResponse{TBody}" /> that wraps the provided
    ///     failure REST  response.
    /// </summary>
    /// <param name="restResponse">The underlying failure REST response for the instance to be created.</param>
    /// <returns>A new <see cref="RestResponse" /> instance.</returns>
    /// <exception cref="ArgumentException">The <paramref name="restResponse" /> argument is successful.</exception>
    public static SuccessOrFailureRestResponse<TBody> FromFailureRestResponse(
        RestResponse<ProblemDetails> restResponse
    ) =>
        restResponse.IsSuccessful
            ? throw new ArgumentException("REST response is successful.")
            : new SuccessOrFailureRestResponse<TBody>(null, restResponse);
}
