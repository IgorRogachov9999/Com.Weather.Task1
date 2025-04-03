using Com.Weather.Task1.Domain.Helpers;
using FluentAssertions;

namespace Com.Weather.Task1.Domain.Tests.Helpers
{
    public class TableHelperTests
    {
        [Fact]
        public void GetParitionKey_Should_ReturnPartitionKey()
        {
            // Arrange
            var date = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero);

            // Act
            var result = TableHelper.GetParitionKey(date);

            // Assert
            result.Should().Be("2025-01");
        }

        [Fact]
        public void GetParitionKey_Should_ReturnPartitionKey_When_RowKey()
        {
            // Arrange
            var rowKey = "2025-01-01-01-01";

            // Act
            var result = TableHelper.GetParitionKey(rowKey);

            // Assert
            result.Should().Be("2025-01");
        }

        [Fact]
        public void GetRowKey_Should_ReturnRowKey()
        {
            // Arrange
            var date = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero);

            // Act
            var result = TableHelper.GetRowKey(date);

            // Assert
            result.Should().Be("2025-01-01-01-01");
        }

        [Fact]
        public void GetBlobNameByRowKey_Should_ReturnRowKey()
        {
            // Arrange
            var rowKey = "2025-01-01-01-01";

            // Act
            var result = TableHelper.GetBlobNameByRowKey(rowKey);

            // Assert
            result.Should().Be("2025-01-01-01-01.json");
        }
    }
}
