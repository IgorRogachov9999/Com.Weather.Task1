using AutoFixture.AutoMoq;
using AutoFixture;
using Com.Weather.Task1.Domain.Services.Contracts;
using Com.Weather.Task1.Domain.Services;
using Moq;
using Com.Weather.Task1.Domain.Entities;
using Com.Weather.Task1.Domain.Dto;
using System.Text.Json;
using FluentAssertions;
using Com.Weather.Task1.Domain.Exceptions;

namespace Com.Weather.Task1.Domain.Tests.Services
{
    public class WeatherServiceTests
    {
        private readonly Mock<ITableService> _tableService;
        private readonly Mock<IStorageService> _storageService;
        private readonly IFixture _fixture;

        private readonly WeatherService _service;

        public WeatherServiceTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            _tableService = _fixture.Freeze<Mock<ITableService>>();
            _storageService = _fixture.Freeze<Mock<IStorageService>>();

            _service = _fixture.Create<WeatherService>();
        }

        [Fact]
        public async Task GetAsync_Should_ReturnWeatherResponseDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var rowKey = "2025-01-01-01-01";
            var weatherRequestInfo = _fixture.Create<WeatherRequestInfo>();
            var weatherResponse = _fixture.Create<WeatherResponseDto>();

            _tableService
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), ct))
                .ReturnsAsync(weatherRequestInfo);
            _storageService
                .Setup(x => x.GetWeatherDataAsync(It.IsAny<string>(), ct))
                .ReturnsAsync(weatherResponse);

            // Act
            var result = await _service.GetAsync(rowKey, ct);

            // Assert
            JsonSerializer.Serialize(result).Should().Be(JsonSerializer.Serialize(weatherResponse));   
        }

        [Fact]
        public async Task GetAsync_Should_ThrowEntityNotFoundException_When_NoData()
        {
            // Arrange
            var ct = CancellationToken.None;
            var rowKey = "2025-01-01-01-01";
            _tableService
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), ct))
                .ReturnsAsync((WeatherRequestInfo)null);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await _service.GetAsync(rowKey, ct));
        }

        [Fact]
        public async Task GetAsync_Should_ReturnWeatherResponseDto_WhenRangeIsValid()
        {
            // Arrange
            var ct = CancellationToken.None;
            var from = new DateTimeOffset(2024, 1, 1, 1, 1, 1, TimeSpan.Zero);
            var to = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero);
            var weatherRequestInfo = _fixture.CreateMany<WeatherRequestInfo>();
            var weatherResponse = _fixture.CreateMany<WeatherResponseDto>();

            _tableService
                .Setup(x => x.GetAsync(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), ct))
                .ReturnsAsync(weatherRequestInfo);
            _storageService
                .Setup(x => x.GetWeatherDataAsync(It.IsAny<string[]>(), ct))
                .ReturnsAsync(weatherResponse);

            // Act
            var result = await _service.GetAsync(from, to, ct);

            // Assert
            result.Count().Should().Be(weatherResponse.Count());
        }

        [Fact]
        public async Task GetAsync_Should_ThrowEntityNotFoundException_When_NoDataForRange()
        {
            // Arrange
            var ct = CancellationToken.None;
            var from = new DateTimeOffset(2024, 1, 1, 1, 1, 1, TimeSpan.Zero);
            var to = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero);
            _tableService
                .Setup(x => x.GetAsync(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), ct))
                .ReturnsAsync(Enumerable.Empty<WeatherRequestInfo>());

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await _service.GetAsync(from, to, ct));
        }

        [Fact]
        public async Task GetAsync_Should_ThrowInvalidArgumentException_When_FromIsGraterTheTo()
        {
            // Arrange
            var ct = CancellationToken.None;
            var from = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero);
            var to = new DateTimeOffset(2024, 1, 1, 1, 1, 1, TimeSpan.Zero);
            _tableService
                .Setup(x => x.GetAsync(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), ct))
                .ReturnsAsync(Enumerable.Empty<WeatherRequestInfo>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidArgumentException>(
                async () => await _service.GetAsync(from, to, ct));
        }
    }
}
