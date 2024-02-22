using DataMatch.Models;
using FluentAssertions;

namespace DataMatch.Tests.Models
{
    public class DiffModelTests
    {
        [Theory]
        [InlineData("AQABAQ==", "AAA=")]
        [InlineData("AAA=", "AQABAQ=")]
        public void Compare_SizeDoNotMatch_ReturnsCorrectResponse(string left, string right)
        {
            // Arrange
            var diffModel = new DiffModel { Left = "AQABAQ==", Right = "AAA=" };

            // Act
            var result = diffModel.Compare();

            // Assert
            result.DiffResultType.Should().Be("SizeDoNotMatch");
            result.Diffs.Should().BeNull();
        }

        [Theory]
        [InlineData("AQABAQ==", "AQABAQ==")]
        public void Compare_Equals_ReturnsCorrectResponse(string left, string right)
        {
            // Arrange
            var diffModel = new DiffModel { Left = left, Right = right };

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
            var diffModel = new DiffModel { Left = "AQABAQ==", Right = "AAAAAA==" };

            // Act
            var result = diffModel.Compare();

            // Assert
            result.DiffResultType.Should().Be("ContentDoNotMatch");
            result.Diffs.Should().NotBeNull();
            result.Diffs.Should().HaveCount(2);
            result.Diffs[0].Offset.Should().Be(0);
            result.Diffs[0].Length.Should().Be(1);
            result.Diffs[1].Offset.Should().Be(2);
            result.Diffs[1].Length.Should().Be(2);
        }
    }
}
