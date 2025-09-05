using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class ContestIdConverter : ValueConverter<ContestId, Guid>
{
    public ContestIdConverter() : base(src => src.Value, value => ContestId.FromValue(value)) { }
}
