using Eurocentric.Domain.Functional;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Components.ErrorHandling;

internal static class DomainErrorExtensions
{
    internal static ProblemDetails ToProblemDetails(this IDomainError domainError)
    {
        return domainError switch
        {
            NotFoundError notFound => MapToProblemDetails(notFound),
            ConflictError conflict => MapToProblemDetails(conflict),
            UnprocessableError unprocessable => MapToProblemDetails(unprocessable),
            _ => throw new InvalidOperationException($"Unsupported domain error type: {domainError.GetType()}."),
        };
    }

    private static ProblemDetails MapToProblemDetails(NotFoundError error)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = error.Title,
            Detail = error.Detail,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            Extensions = error.Extensions?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [],
        };
    }

    private static ProblemDetails MapToProblemDetails(ConflictError error)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = error.Title,
            Detail = error.Detail,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            Extensions = error.Extensions?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [],
        };
    }

    private static ProblemDetails MapToProblemDetails(UnprocessableError error)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = error.Title,
            Detail = error.Detail,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.21",
            Extensions = error.Extensions?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [],
        };
    }
}
