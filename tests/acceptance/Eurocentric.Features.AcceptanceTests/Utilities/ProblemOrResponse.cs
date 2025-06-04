using Microsoft.AspNetCore.Mvc;
using OneOf;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

/// <summary>
///     Contains <i>EITHER</i> an unsuccessful rest response with a <see cref="ProblemDetails" /> response object <i>OR</i>
///     a successful REST response with no response object.
/// </summary>
public sealed class ProblemOrResponse : OneOfBase<RestResponse<ProblemDetails>, RestResponse>
{
    internal ProblemOrResponse(OneOf<RestResponse<ProblemDetails>, RestResponse> input) : base(input)
    {
    }

    /// <summary>
    ///     Gets the unsuccessful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsProblem => AsT0;

    /// <summary>
    ///     Gets the successful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse AsResponse => AsT1;
}

/// <summary>
///     Contains <i>EITHER</i> an unsuccessful rest response with a <see cref="ProblemDetails" /> response object <i>OR</i>
///     a successful REST response with a response object of type <typeparamref name="TResponse" />.
/// </summary>
public sealed class ProblemOrResponse<TResponse> : OneOfBase<RestResponse<ProblemDetails>, RestResponse<TResponse>>
{
    internal ProblemOrResponse(OneOf<RestResponse<ProblemDetails>, RestResponse<TResponse>> input) : base(input)
    {
    }

    /// <summary>
    ///     Gets the unsuccessful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsProblem => AsT0;

    /// <summary>
    ///     Gets the successful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse<TResponse> AsResponse => AsT1;
}
