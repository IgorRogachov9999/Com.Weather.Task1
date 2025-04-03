using AutoFixture;
using AutoFixture.AutoMoq;
using Com.Weather.Task1.Domain.Dto;
using Com.Weather.Task1.Domain.Entities;
using Com.Weather.Task1.Domain.Services;
using Com.Weather.Task1.Domain.Services.Contracts;
using Moq;

namespace Com.Weather.Task1.Domain.Tests.Services
{
    public class WeatherPollerServiceTests
    {
        private readonly Mock<IOpenWeatherMapClientService> _openWeatherMapClientService;
        private readonly Mock<ITableService> _tableService;
        private readonly Mock<IStorageService> _storageService;
        private readonly IFixture _fixture;

        private readonly WeatherPollerService _service;

        public WeatherPollerServiceTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            _openWeatherMapClientService = _fixture.Freeze<Mock<IOpenWeatherMapClientService>>();
            _tableService = _fixture.Freeze<Mock<ITableService>>();
            _storageService = _fixture.Freeze<Mock<IStorageService>>();

            _service = _fixture.Create<WeatherPollerService>();
        }

        [Fact]
        public async Task PollWeatherAsync_Should_SaveBlob_When_HasData()
        {
            // Arrange
            var ct = CancellationToken.None;
            var weatherResponse = _fixture.Create<WeatherResponseDto>();
            _openWeatherMapClientService
                .Setup(x => x.GetWeatherDataAsync(ct))
                .ReturnsAsync(weatherResponse);

            // Act
            await _service.PollWeatherAsync(ct);

            // Assert
            _openWeatherMapClientService.Verify(x => x.GetWeatherDataAsync(ct), Times.Once);
            _storageService.Verify(x => x.SaveAsync(It.IsAny<WeatherResponseDto>(), It.IsAny<string>(), ct), Times.Once());
            _tableService.Verify(x => x.SaveAsync(It.IsAny<WeatherRequestInfo>(), ct), Times.Once());
        }

        [Fact]
        public async Task PollWeatherAsync_Should_NotSaveBlob_When_NoData()
        {
            // Arrange
            var ct = CancellationToken.None;
            _openWeatherMapClientService
                .Setup(x => x.GetWeatherDataAsync(ct))
                .ReturnsAsync((WeatherResponseDto)null);

            // Act
            await _service.PollWeatherAsync(ct);

            // Assert
            _openWeatherMapClientService.Verify(x => x.GetWeatherDataAsync(ct), Times.Once);
            _storageService.Verify(x => x.SaveAsync(It.IsAny<WeatherResponseDto>(), It.IsAny<string>(), ct), Times.Never());
            _tableService.Verify(x => x.SaveAsync(It.IsAny<WeatherRequestInfo>(), ct), Times.Once());
        }
    }
}
