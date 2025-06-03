using Microsoft.AspNetCore.Mvc;
using OneOf;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

/// <summary>
///     Contains <i>EITHER</i> a successful REST response with no response object <i>OR</i> an unsuccessful rest response
///     with a <see cref="ProblemDetails" /> response object.
/// </summary>
public sealed class ResponseOrProblem : OneOfBase<RestResponse, RestResponse<ProblemDetails>>
{
    internal ResponseOrProblem(OneOf<RestResponse, RestResponse<ProblemDetails>> input) : base(input)
    {
    }

    /// <summary>
    ///     Gets the underlying successful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse AsResponse => AsT0;

    /// <summary>
    ///     Gets the underlying unsuccessful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsProblem => AsT1;
}

/// <summary>
///     Contains <i>EITHER</i> a successful REST response with a response object of type <typeparamref name="TResponse" />
///     <i>OR</i> an unsuccessful rest response with a <see cref="ProblemDetails" /> response object.
/// </summary>
public sealed class ResponseOrProblem<TResponse> : OneOfBase<RestResponse<TResponse>, RestResponse<ProblemDetails>>
{
    internal ResponseOrProblem(OneOf<RestResponse<TResponse>, RestResponse<ProblemDetails>> input) : base(input)
    {
    }

    /// <summary>
    ///     Gets the underlying successful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is unsuccessful.</exception>
    public RestResponse<TResponse> AsResponse => AsT0;

    /// <summary>
    ///     Gets the underlying unsuccessful REST response.
    /// </summary>
    /// <exception cref="InvalidOperationException">The underlying REST response is successful.</exception>
    public RestResponse<ProblemDetails> AsProblem => AsT1;
}
