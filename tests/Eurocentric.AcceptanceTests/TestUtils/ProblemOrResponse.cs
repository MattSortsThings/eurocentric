using Microsoft.AspNetCore.Mvc;
using OneOf;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     The discriminated union of <i>either</i> an unsuccessful REST response with a deserialized
///     <see cref="ProblemDetails" /> response body <i>or</i> a successful REST response.
/// </summary>
public sealed class ProblemOrResponse : OneOfBase<RestResponse<ProblemDetails>, RestResponse>
{
    private ProblemOrResponse(OneOf<RestResponse<ProblemDetails>, RestResponse> input)
        : base(input) { }

    /// <summary>
    ///     Gets the underlying unsuccessful REST response.
    /// </summary>
    public RestResponse<ProblemDetails> AsProblem => AsT0;

    /// <summary>
    ///     Gets the underlying successful REST response.
    /// </summary>
    public RestResponse AsResponse => AsT1;

    /// <summary>
    ///     Creates and returns a new <see cref="ProblemOrResponse" /> instance wrapping the provided unsuccessful REST
    ///     response.
    /// </summary>
    /// <param name="problem">The unsuccessful REST response.</param>
    /// <returns>A new <see cref="ProblemOrResponse" /> object.</returns>
    public static ProblemOrResponse FromProblem(RestResponse<ProblemDetails> problem) => new(problem);

    /// <summary>
    ///     Creates and returns a new <see cref="ProblemOrResponse" /> instance wrapping the provided successful REST response.
    /// </summary>
    /// <param name="response">The successful REST response object.</param>
    /// <returns>A new <see cref="ProblemOrResponse" /> object.</returns>
    public static ProblemOrResponse FromResponse(RestResponse response) => new(response);
}
