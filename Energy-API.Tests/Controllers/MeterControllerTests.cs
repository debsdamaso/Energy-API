using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Energy_API.Controllers;
using Energy_API.Services.Interfaces;
using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MeterControllerTests
{
    private readonly Mock<IMeterService> _mockMeterService;
    private readonly MeterController _meterController;

    public MeterControllerTests()
    {
        _mockMeterService = new Mock<IMeterService>();
        _meterController = new MeterController(_mockMeterService.Object);
    }

    [Fact]
    public async Task GetAllMeters_ShouldReturn_OkWithMeters()
    {
        // Arrange
        var meters = new List<Meter> { new Meter { Id = "1", Location = "Living Room" } };
        _mockMeterService.Setup(s => s.GetAllMetersAsync()).ReturnsAsync(meters);

        // Act
        var result = await _meterController.GetAllMeters();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMeters = Assert.IsAssignableFrom<List<Meter>>(okResult.Value);
        Assert.Single(returnedMeters);
    }

    [Fact]
    public async Task GetMeterById_ShouldReturn_OkWithMeter_WhenExists()
    {
        // Arrange
        var meter = new Meter { Id = "1", Location = "Kitchen" };
        _mockMeterService.Setup(s => s.GetMeterByIdAsync("1")).ReturnsAsync(meter);

        // Act
        var result = await _meterController.GetMeterById("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMeter = Assert.IsType<Meter>(okResult.Value);
        Assert.Equal("1", returnedMeter.Id);
        Assert.Equal("Kitchen", returnedMeter.Location);
    }

    [Fact]
    public async Task GetMeterById_ShouldReturn_NotFound_WhenMeterDoesNotExist()
    {
        // Arrange
        _mockMeterService.Setup(s => s.GetMeterByIdAsync("2")).ReturnsAsync((Meter?)null);

        // Act
        var result = await _meterController.GetMeterById("2");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateMeter_ShouldReturn_CreatedAtAction()
    {
        // Arrange
        var newMeter = new Meter { Id = "1", Location = "Garage" };
        _mockMeterService.Setup(s => s.CreateMeterAsync(newMeter)).Returns(Task.CompletedTask);

        // Act
        var result = await _meterController.CreateMeter(newMeter);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdMeter = Assert.IsType<Meter>(createdAtActionResult.Value);
        Assert.Equal("Garage", createdMeter.Location);
    }

    [Fact]
    public async Task UpdateMeter_ShouldReturn_NoContent()
    {
        // Arrange
        var updatedMeter = new Meter { Id = "1", Location = "Office" };
        _mockMeterService.Setup(s => s.UpdateMeterAsync("1", updatedMeter)).Returns(Task.CompletedTask);

        // Act
        var result = await _meterController.UpdateMeter("1", updatedMeter);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteMeter_ShouldReturn_NoContent()
    {
        // Arrange
        _mockMeterService.Setup(s => s.DeleteMeterAsync("1")).Returns(Task.CompletedTask);

        // Act
        var result = await _meterController.DeleteMeter("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
