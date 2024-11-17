using Energy_API.Controllers;
using Energy_API.DTOs;
using Energy_API.Models;
using Energy_API.Repositories.Interfaces;
using Energy_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Energy_API.Tests.Controllers
{
    public class ReportControllerTests
    {
        private readonly Mock<IDeviceRepository> _mockDeviceRepository;
        private readonly Mock<IMeterRepository> _mockMeterRepository;
        private readonly DeviceService _deviceService;
        private readonly MeterService _meterService;
        private readonly ReportController _reportController;

        public ReportControllerTests()
        {
            _mockDeviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
            _mockMeterRepository = new Mock<IMeterRepository>(MockBehavior.Strict);
            _deviceService = new DeviceService(_mockDeviceRepository.Object);
            _meterService = new MeterService(_mockMeterRepository.Object);
            _reportController = new ReportController(_deviceService, _meterService);
        }

        [Fact]
        public async Task GetConsumptionByLocation_NoMetersFound_ReturnsEmptyList()
        {
            // Arrange
            _mockMeterRepository.Setup(repo => repo.GetAllMetersAsync()).ReturnsAsync(new List<Meter>());

            // Act
            var result = await _reportController.GetConsumptionByLocation();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var data = okResult.Value as IEnumerable<ConsumptionByLocationDto>;
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetInefficientDevices_NoDevicesFound_ReturnsEmptyList()
        {
            // Arrange
            _mockDeviceRepository.Setup(repo => repo.GetAllDevicesAsync()).ReturnsAsync(new List<Device>());

            // Act
            var result = await _reportController.GetInefficientDevices();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var data = okResult.Value as IEnumerable<InefficientDeviceDto>;
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetDevicesWithAnomalousConsumption_NoDevicesFound_ReturnsEmptyList()
        {
            // Arrange
            _mockDeviceRepository.Setup(repo => repo.GetAllDevicesAsync()).ReturnsAsync(new List<Device>());

            // Act
            var result = await _reportController.GetDevicesWithAnomalousConsumption();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var data = okResult.Value as IEnumerable<AnomalousConsumptionDto>;
            Assert.NotNull(data);
            Assert.Empty(data);
        }
    }
}
