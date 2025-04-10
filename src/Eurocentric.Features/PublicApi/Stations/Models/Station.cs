using Eurocentric.Domain.Placeholders;

namespace Eurocentric.Features.PublicApi.Stations.Models;

public sealed record Station(string Name, Line Line);
