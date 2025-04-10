using Eurocentric.Domain.Placeholders;

namespace Eurocentric.Features.PublicApi.V0.Stations.Models;

public sealed record Station(string Name, Line Line);
