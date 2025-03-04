using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Eurocentric.Shared.Documentation;

internal static class ApiDescriptionExtensions
{
    internal static string GetInstance(this ApiDescription subject) => $"{subject.HttpMethod} {subject.RelativePath}";
}
