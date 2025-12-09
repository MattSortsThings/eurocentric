using Eurocentric.Tests.Unit.Utils;

namespace Eurocentric.Tests.Unit;

[Category("placeholder")]
public sealed class PlaceholderUnitTests : UnitTests
{
    private static int Add(int x, int y) => x + y;

    [Test]
    [MatrixDataSource]
    public async Task Add_should_return_number_greater_than_or_equal_to_each_argument(
        [MatrixRange<int>(1, 10)] int value1,
        [MatrixRange<int>(1, 10)] int value2
    )
    {
        int result = Add(value1, value2);

        await Assert.That(result).IsGreaterThanOrEqualTo(value1).And.IsGreaterThanOrEqualTo(value2);
    }
}
