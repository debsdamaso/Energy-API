using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Energy_API.Controllers;
using Energy_API.Services.Interfaces;
using Energy_API.DTOs;

public class EfficiencyControllerTests
{
    private readonly Mock<IEfficiencyService> _mockEfficiencyService;
    private readonly EfficiencyController _efficiencyController;

    public EfficiencyControllerTests()
    {
        _mockEfficiencyService = new Mock<IEfficiencyService>();
        _efficiencyController = new EfficiencyController(_mockEfficiencyService.Object);
    }

    [Fact]
    public void ClassifyDevice_ShouldReturn_EfficiencyClass()
    {
        // Arrange
        var deviceUsage = new DeviceUsageDto
        {
            DeviceType = "Refrigerador",
            MonthlyUsage = 80
        };

        _mockEfficiencyService
            .Setup(s => s.ClassifyDeviceEfficiency(deviceUsage.DeviceType, deviceUsage.MonthlyUsage))
            .Returns("A");

        // Act
        var result = _efficiencyController.ClassifyDevice(deviceUsage);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<EfficiencyResponseDto>(okResult.Value);
        Assert.Equal("A", response.EfficiencyClass);
    }
}
