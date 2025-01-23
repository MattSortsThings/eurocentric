using Asp.Versioning;

namespace Eurocentric.Shared.ApiModules;

internal sealed record ApiRelease(ApiVersion ApiVersion, string GroupName, IApiEndpoint[] ApiEndpoints);
