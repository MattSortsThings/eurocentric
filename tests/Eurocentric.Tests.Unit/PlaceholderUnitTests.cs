using Eurocentric.Tests.Unit.Utils;

namespace Eurocentric.Tests.Unit;

public sealed class PlaceholderUnitTests
{
    private static int Add(int x, int y) => x + y;

    [Category("placeholder")]
    public sealed class Addition : UnitTests
    {
        [Test]
        [MatrixDataSource]
        public async Task Should_return_number_greater_than_or_equal_to_each_argument(
            [MatrixRange<int>(1, 10)] int value1,
            [MatrixRange<int>(1, 10)] int value2
        )
        {
            int result = Add(value1, value2);

            await Assert.That(result).IsGreaterThanOrEqualTo(value1).And.IsGreaterThanOrEqualTo(value2);
        }
    }
}
