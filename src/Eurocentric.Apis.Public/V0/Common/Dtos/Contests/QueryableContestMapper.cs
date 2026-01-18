using Eurocentric.Domain.Queries.Placeholders;
using Riok.Mapperly.Abstractions;

namespace Eurocentric.Apis.Public.V0.Common.Dtos.Contests;

[Mapper]
internal static partial class QueryableContestMapper
{
    internal static partial Contest ToContestDto(QueryableContest queryableContest);
}
