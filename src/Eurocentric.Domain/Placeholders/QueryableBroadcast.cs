using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders;

public sealed record QueryableBroadcast(int ContestYear, ContestStage ContestStage, DateOnly BroadcastDate, bool TelevoteOnly);
