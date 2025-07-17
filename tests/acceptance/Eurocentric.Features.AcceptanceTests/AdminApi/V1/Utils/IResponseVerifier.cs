using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IResponseVerifier
{
    public HttpStatusCode ResponseStatusCode { get; }

    public ProblemDetails? ResponseProblemDetails { get; }
}

public interface IResponseVerifier<out T> : IResponseVerifier
    where T : class
{
    public T? ResponseObject { get; }
}
