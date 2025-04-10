using Eurocentric.Domain.Placeholders;

namespace Eurocentric.Features.AdminApi.Stations.Models;

public sealed record Station(int Id, string Name, Line Line);
