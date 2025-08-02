using Microsoft.AspNetCore.Mvc;
using OneOf;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

/// <summary>
///     Contains <i>EITHER</i> an unsuccessful REST response with a <see cref="ProblemDetails" /> response object
///     deserialized from the response body <i>OR</i> a successful REST response with no deserialized response object.
/// </summary>
public sealed class ProblemOrResponse : OneOfBase<RestResponse<ProblemDetails>, RestResponse>
{
    public ProblemOrResponse(OneOf<RestResponse<ProblemDetails>, RestResponse> input) : base(input)
    {
    }

    /// <summary>
    ///     Gets the unsuccessful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsProblem
    {
        get
        {
            try
            {
                return AsT0;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot return as Unsuccessful because instance is Response.", ex);
            }
        }
    }

    /// <summary>
    ///     Gets the successful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse AsResponse
    {
        get
        {
            try
            {
                return AsT1;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot return as Successful because instance is Problem.", ex);
            }
        }
    }
}

/// <summary>
///     Contains <i>EITHER</i> an unsuccessful REST response with a <see cref="ProblemDetails" /> response object
///     deserialized from the response body <i>OR</i> a successful REST response with a response of type
///     <typeparamref name="TResponse" /> deserialized from the response body.
/// </summary>
public sealed class ProblemOrResponse<TResponse> : OneOfBase<RestResponse<ProblemDetails>, RestResponse<TResponse>>
    where TResponse : class
{
    public ProblemOrResponse(OneOf<RestResponse<ProblemDetails>, RestResponse<TResponse>> input) : base(input)
    {
    }

    /// <summary>
    ///     Gets the unsuccessful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsProblem
    {
        get
        {
            try
            {
                return AsT0;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot return as Unsuccessful because instance is Response.", ex);
            }
        }
    }

    /// <summary>
    ///     Gets the successful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse<TResponse> AsResponse
    {
        get
        {
            try
            {
                return AsT1;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot return as Successful because instance is Problem.", ex);
            }
        }
    }
}
