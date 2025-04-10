using Eurocentric.Domain.Placeholders;

namespace Eurocentric.Features.AdminApi.V0.Stations.Models;

public sealed record Station(int Id, string Name, Line Line);
