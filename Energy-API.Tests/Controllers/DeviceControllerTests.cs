using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Energy_API.Controllers;
using Energy_API.Services.Interfaces;
using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DeviceControllerTests
{
    private readonly Mock<IDeviceService> _mockDeviceService;
    private readonly DeviceController _deviceController;

    public DeviceControllerTests()
    {
        // Mock para a interface, não para a classe concreta
        _mockDeviceService = new Mock<IDeviceService>();
        _deviceController = new DeviceController(_mockDeviceService.Object);
    }

    [Fact]
    public async Task GetAllDevices_ShouldReturn_OkWithDevices()
    {
        // Arrange
        var devices = new List<Device> { new Device { Id = "1", Name = "Device 1", CurrentWatts = 100 } };
        _mockDeviceService.Setup(s => s.GetAllDevicesAsync()).ReturnsAsync(devices);

        // Act
        var result = await _deviceController.GetAllDevices();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnDevices = Assert.IsAssignableFrom<List<Device>>(okResult.Value);
        Assert.Single(returnDevices);
    }

    [Fact]
    public async Task GetDeviceById_ShouldReturn_NotFound_WhenDeviceDoesNotExist()
    {
        // Arrange
        _mockDeviceService.Setup(s => s.GetDeviceByIdAsync("2")).ReturnsAsync((Device?)null);

        // Act
        var result = await _deviceController.GetDeviceById("2");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
