using Microsoft.AspNetCore.Mvc;
using OneOf;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     TThe discriminated union of <i>either</i> an unsuccessful REST response with a deserialized
///     <see cref="ProblemDetails" /> response body <i>or</i> a successful REST response with a deserialized response body
///     of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The successful response body type.</typeparam>
public sealed class ProblemOrResponse<T> : OneOfBase<RestResponse<ProblemDetails>, RestResponse<T>>
    where T : class
{
    public ProblemOrResponse(OneOf<RestResponse<ProblemDetails>, RestResponse<T>> input)
        : base(input) { }

    /// <summary>
    ///     Gets the underlying unsuccessful REST response.
    /// </summary>
    public RestResponse<ProblemDetails> AsProblem
    {
        get
        {
            try
            {
                return AsT0;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("value was Response");
            }
        }
    }

    /// <summary>
    ///     Gets the underlying successful REST response.
    /// </summary>
    public RestResponse<T> AsResponse
    {
        get
        {
            try
            {
                return AsT1;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("value was Problem");
            }
        }
    }

    /// <summary>
    ///     Creates and returns a new <see cref="ProblemOrResponse{T}" /> instance wrapping the provided unsuccessful REST
    ///     response.
    /// </summary>
    /// <param name="problem">The unsuccessful REST response.</param>
    /// <returns>A new <see cref="ProblemOrResponse{T}" /> object.</returns>
    public static ProblemOrResponse<T> FromProblem(RestResponse<ProblemDetails> problem) => new(problem);

    /// <summary>
    ///     Creates and returns a new <see cref="ProblemOrResponse" /> instance wrapping the provided successful REST response.
    /// </summary>
    /// <param name="response">The successful REST response object.</param>
    /// <returns>A new <see cref="ProblemOrResponse" /> object.</returns>
    public static ProblemOrResponse<T> FromResponse(RestResponse<T> response) => new(response);
}
