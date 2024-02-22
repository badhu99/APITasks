using DataMatch.Models;
using FluentAssertions;

namespace DataMatch.Tests.Models
{
    public class DiffModelTests
    {
        [Fact]
        public void Compare_SizeDoNotMatch_ReturnsCorrectResponse()
        {
            // Arrange
            var diffModel = new DiffModel { Left = "ABC", Right = "ABCD" };

            // Act
            var result = diffModel.Compare();

            // Assert
            result.DiffResultType.Should().Be("SizeDoNotMatch");
            result.Diffs.Should().BeNull();
        }

        [Fact]
        public void Compare_Equals_ReturnsCorrectResponse()
        {
            // Arrange
            var diffModel = new DiffModel { Left = "ABC", Right = "ABC" };

            // Act
            var result = diffModel.Compare();

            // Assert
            result.DiffResultType.Should().Be("Equals");
            result.Diffs.Should().BeNull();
        }

        [Fact]
        public void Compare_ContentDoNotMatch_ReturnsCorrectResponse()
        {
            // Arrange
            var diffModel = new DiffModel { Left = "ABCDEF", Right = "ABZZZF" };

            // Act
            var result = diffModel.Compare();

            // Assert
            result.DiffResultType.Should().Be("ContentDoNotMatch");
            result.Diffs.Should().NotBeNull();
            result.Diffs.Should().HaveCount(1);
            result.Diffs.Should().ContainSingle(diff => diff.Offset == 2 && diff.Length == 3);
        }
    }
}
