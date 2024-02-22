using DataMatch.Enums;
using DataMatch.Models;
using DataMatch.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace DataMatch.Tests.Services
{
    public class DiffServicesTests
    {
        private readonly MemoryCache _memoryCache;
        public DiffServicesTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());            
        }

        [Theory]
        [InlineData(1, DiffDirection.Left, "AQABAQ==")]
        [InlineData(1, DiffDirection.Right, "AQABAQ==")]
        public void SetData_ValidId_ReturnsVoid(int id, DiffDirection direction, string data)
        {
            // Arrange
            var diffService = new DiffService(_memoryCache);

            // Act
            diffService.SetData(id, direction, data);

            // Assert
            _memoryCache.Count.Should().Be(1);
        }

        [Theory]
        [InlineData("AQABAQ==", true)]
        [InlineData("AAA==", false)]
        public void ValidateBase64Encoded__ReturnsBool(string data, bool result)
        {
            // Arrange
            var diffService = new DiffService(_memoryCache);

            // Act
            bool boolValidString = diffService.ValidateBase64Encoded(data);

            // Assert
            boolValidString.Should().Be(result);
        }

        [Fact]
        public void DataMatch_ContentDoNotMatch_ReturnsDataMatchResponse()
        {
            // Arrange
            var id = 1;
            var model = new DiffModel()
            {
                Left = "AQABAQ==",
                Right = "AAAAAA==",
            };

            _memoryCache.Set(id, model);
            var diffService = new DiffService(_memoryCache);

            // Act
            var result = diffService.DataMatch(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<DataMatchResponse>();
            result.DiffResultType = "ContentDoNotMatch";
            result.Diffs.Should().NotBeNullOrEmpty().And.HaveCountGreaterThan(1);
        }

        [Fact]
        public void DataMatch_Equals_ReturnsDataMatchResponse()
        {
            // Arrange
            var id = 1;
            var model = new DiffModel()
            {
                Left = "AAAAAA==",
                Right = "AAAAAA==",
            };

            _memoryCache.Set(id, model);
            var diffService = new DiffService(_memoryCache);

            // Act
            var result = diffService.DataMatch(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<DataMatchResponse>();
            result.DiffResultType = "Equals";
        }

        [Fact]
        public void DataMatch_SizeDoNotMatch_ReturnsDataMatchResponse()
        {
            // Arrange
            var id = 1;
            var model = new DiffModel()
            {
                Left = "AAAAAA==",
                Right = "AAA=",
            };

            _memoryCache.Set(id, model);
            var diffService = new DiffService(_memoryCache);

            // Act
            var result = diffService.DataMatch(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<DataMatchResponse>();
            result.DiffResultType = "SizeDoNotMatch";
        }
    }
}
