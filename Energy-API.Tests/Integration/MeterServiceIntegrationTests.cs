using Xunit;
using Moq;
using Energy_API.Services;
using Energy_API.Repositories.Interfaces;
using Energy_API.Models;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Energy_API.Tests.Integration
{
    public class MeterServiceIntegrationTests
    {
        private readonly Mock<IMeterRepository> _mockMeterRepository;
        private readonly MeterService _meterService;

        public MeterServiceIntegrationTests()
        {
            _mockMeterRepository = new Mock<IMeterRepository>();
            _meterService = new MeterService(_mockMeterRepository.Object);
        }

        [Fact]
        public async Task GetAllMeters_ShouldCallRepositoryAndReturnMeters()
        {
            // Arrange
            var meters = new List<Meter>
            {
                new Meter { Id = ObjectId.GenerateNewId().ToString(), Location = "Living Room" },
                new Meter { Id = ObjectId.GenerateNewId().ToString(), Location = "Kitchen" }
            };

            _mockMeterRepository
                .Setup(repo => repo.GetAllMetersAsync())
                .ReturnsAsync(meters);

            // Act
            var result = await _meterService.GetAllMetersAsync();

            // Assert
            Assert.Equal(2, result.Count);
            _mockMeterRepository.Verify(repo => repo.GetAllMetersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetMeterById_ShouldCallRepositoryAndReturnMeter()
        {
            // Arrange
            var validObjectId = ObjectId.GenerateNewId().ToString();
            var meter = new Meter { Id = validObjectId, Location = "Living Room" };

            _mockMeterRepository
                .Setup(repo => repo.GetMeterByIdAsync(validObjectId))
                .ReturnsAsync(meter);

            // Act
            var result = await _meterService.GetMeterByIdAsync(validObjectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Living Room", result.Location);
            _mockMeterRepository.Verify(repo => repo.GetMeterByIdAsync(validObjectId), Times.Once);
        }

        [Fact]
        public async Task CreateMeter_ShouldCallRepository()
        {
            // Arrange
            var newMeter = new Meter { Location = "Garage" };

            _mockMeterRepository
                .Setup(repo => repo.CreateMeterAsync(It.IsAny<Meter>()))
                .Returns(Task.CompletedTask);

            // Act
            await _meterService.CreateMeterAsync(newMeter);

            // Assert
            _mockMeterRepository.Verify(repo => repo.CreateMeterAsync(It.IsAny<Meter>()), Times.Once);
        }

        [Fact]
        public async Task DeleteMeter_ShouldCallRepository()
        {
            // Arrange
            var validObjectId = ObjectId.GenerateNewId().ToString();
            var existingMeter = new Meter { Id = validObjectId, Location = "Garage" };

            _mockMeterRepository
                .Setup(repo => repo.GetMeterByIdAsync(validObjectId))
                .ReturnsAsync(existingMeter);

            _mockMeterRepository
                .Setup(repo => repo.DeleteMeterAsync(validObjectId))
                .Returns(Task.CompletedTask);

            // Act
            await _meterService.DeleteMeterAsync(validObjectId);

            // Assert
            _mockMeterRepository.Verify(repo => repo.GetMeterByIdAsync(validObjectId), Times.Once);
            _mockMeterRepository.Verify(repo => repo.DeleteMeterAsync(validObjectId), Times.Once);
        }
    }
}
