using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Eurocentric.Shared.ApiRegistration;

internal static class ApiDescriptionExtensions
{
    internal static bool ShouldIncludeIn(this ApiDescription subject, string groupName, ApiVersion apiVersion) =>
        subject.GroupName == groupName && subject.GetApiVersion() == apiVersion;
}
