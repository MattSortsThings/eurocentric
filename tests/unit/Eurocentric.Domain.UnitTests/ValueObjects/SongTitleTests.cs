using ErrorOr;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class SongTitleTests : UnitTest
{
    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        SongTitle nameA = SongTitle.FromValue("Amar Pelos Dois").Value;
        SongTitle nameB = SongTitle.FromValue("Before the Party's Over").Value;
        SongTitle nameC = SongTitle.FromValue("Cha Cha Cha").Value;
        SongTitle nameD = SongTitle.FromValue("Dobrodošli").Value;

        List<SongTitle> sut = [nameC, nameB, nameD, nameA];

        // Act
        sut.Sort(Comparer<SongTitle>.Default);

        // Assert
        await Assert.That(sut).IsEquivalentTo([nameA, nameB, nameC, nameD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        const string sutValue = "Amar Pelos Dois";

        SongTitle sut = SongTitle.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_Value_properties_are_equal()
    {
        // Arrange
        const string sharedValue = "Amar Pelos Dois";

        SongTitle sut = SongTitle.FromValue(sharedValue).Value;
        SongTitle other = SongTitle.FromValue(sharedValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_Value_properties_are_unequal()
    {
        // Arrange
        const string sutValue = "Amar Pelos Dois";
        const string otherValue = "11:11";

        SongTitle sut = SongTitle.FromValue(sutValue).Value;
        SongTitle other = SongTitle.FromValue(otherValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        const string sutValue = "Amar Pelos Dois";

        SongTitle sut = SongTitle.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("A")]
    [Arguments("Amar Pelos Dois")]
    [Arguments("D.G.T. (Off and On)")]
    [Arguments("Blood & Glitter")]
    [Arguments("Who The Hell Is Edgar?")]
    public async Task FromValue_should_return_instance_with_provided_value(string value)
    {
        // Act
        ErrorOr<SongTitle> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(result.Value)
            .HasMember(countryCode => countryCode.Value).EqualTo(value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_empty_string()
    {
        // Arrange
        const string value = "";

        // Act
        ErrorOr<SongTitle> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal song title value")
            .And.HasDescription("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("songTitle", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_all_whitespace()
    {
        // Arrange
        const string value = "   ";

        // Act
        ErrorOr<SongTitle> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal song title value")
            .And.HasDescription("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("songTitle", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_longer_than_200_characters()
    {
        // Arrange
        string value = new('X', 201);

        // Act
        ErrorOr<SongTitle> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal song title value")
            .And.HasDescription("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("songTitle", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Arrange
        Action act = () => SongTitle.FromValue(null!);

        // Assert
        await Assert.That(act)
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')")
            .WithParameterName("value");
    }
}
