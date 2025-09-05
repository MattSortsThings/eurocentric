using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class ContestYearConverter : ValueConverter<ContestYear, int>
{
    public ContestYearConverter() : base(src => src.Value, value => ContestYear.FromValue(value).Value) { }
}
