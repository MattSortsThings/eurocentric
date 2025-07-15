using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public static class ListExtensionsTests
{
    public sealed class CollectExtensionMethod : UnitTest
    {
        [Fact]
        public void Should_return_list_of_values_in_original_order_when_all_items_are_values()
        {
            // Arrange
            List<ErrorOr<string>> sut = ["CZ", "AT", "GB"];

            // Act
            ErrorOr<List<string>> errorsOrList = sut.Collect();

            (bool isError, List<string> list) = (errorsOrList.IsError, errorsOrList.Value);

            // Assert
            Assert.False(isError);

            Assert.Equal(["CZ", "AT", "GB"], list);
        }

        [Fact]
        public void Should_return_list_of_Errors_in_flattened_original_order_when_any_item_is_Errors()
        {
            // Arrange
            Error error = Error.Failure();

            List<ErrorOr<string>> sut = [error, "AT", "GB"];

            // Act
            ErrorOr<List<string>> errorsOrList = sut.Collect();

            (bool isError, List<string> list, List<Error> errors) =
                (errorsOrList.IsError, errorsOrList.Value, errorsOrList.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(list);

            Assert.Equal([error], errors);
        }

        [Fact]
        public void Should_return_list_of_Errors_in_flattened_original_order_when_multiple_items_are_Errors()
        {
            // Arrange
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();
            Error errorD = Error.Unexpected();

            List<ErrorOr<string>> sut = [new List<Error> { errorA, errorD }, "AT", new List<Error> { errorB, errorC }];

            // Act
            ErrorOr<List<string>> errorsOrList = sut.Collect();

            (bool isError, List<string> list, List<Error> errors) =
                (errorsOrList.IsError, errorsOrList.Value, errorsOrList.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(list);

            Assert.Equal([errorA, errorD, errorB, errorC], errors);
        }

        [Fact]
        public void Should_return_list_of_Errors_in_flattened_original_order_when_all_items_are_Errors()
        {
            // Arrange
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();
            Error errorD = Error.Unexpected();

            List<ErrorOr<string>> sut = [new List<Error> { errorA, errorD }, errorC, new List<Error> { errorB }];

            // Act
            ErrorOr<List<string>> errorsOrList = sut.Collect();

            (bool isError, List<string> list, List<Error> errors) =
                (errorsOrList.IsError, errorsOrList.Value, errorsOrList.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(list);

            Assert.Equal([errorA, errorD, errorC, errorB], errors);
        }
    }
}
