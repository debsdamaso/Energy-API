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
    public class DeviceServiceIntegrationTests
    {
        private readonly Mock<IDeviceRepository> _mockDeviceRepository;
        private readonly DeviceService _deviceService;

        public DeviceServiceIntegrationTests()
        {
            _mockDeviceRepository = new Mock<IDeviceRepository>();
            _deviceService = new DeviceService(_mockDeviceRepository.Object);
        }

        [Fact]
        public async Task GetDeviceById_ShouldCallRepositoryAndReturnDevice()
        {
            // Arrange
            var validObjectId = ObjectId.GenerateNewId().ToString();
            var device = new Device
            {
                Id = validObjectId,
                Name = "Smart TV",
                Type = "Electronics"
            };

            _mockDeviceRepository
                .Setup(repo => repo.GetDeviceByIdAsync(validObjectId))
                .ReturnsAsync(device);

            // Act
            var result = await _deviceService.GetDeviceByIdAsync(validObjectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Smart TV", result.Name);
            _mockDeviceRepository.Verify(repo => repo.GetDeviceByIdAsync(validObjectId), Times.Once);
        }

        [Fact]
        public async Task CreateDevice_ShouldCallRepositoryAndReturnCreatedDevice()
        {
            // Arrange
            var newDevice = new Device
            {
                Name = "Air Conditioner",
                Type = "Appliance",
                CurrentWatts = 1200,
                EstimatedUsageHours = 8,
                MonthlyEnergyUsage = 96,
                EfficiencyClass = "A",
                MeterId = ObjectId.GenerateNewId().ToString()
            };

            _mockDeviceRepository
                .Setup(repo => repo.CreateDeviceAsync(It.IsAny<Device>()))
                .Returns(Task.CompletedTask);

            // Act
            await _deviceService.CreateDeviceAsync(newDevice);

            // Assert
            _mockDeviceRepository.Verify(repo => repo.CreateDeviceAsync(It.IsAny<Device>()), Times.Once);
        }
    }
}
