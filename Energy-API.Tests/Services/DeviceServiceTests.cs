using Xunit;
using Moq;
using Energy_API.Services;
using Energy_API.Repositories.Interfaces;
using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _mockDeviceRepository;
    private readonly DeviceService _deviceService;

    public DeviceServiceTests()
    {
        _mockDeviceRepository = new Mock<IDeviceRepository>();
        _deviceService = new DeviceService(_mockDeviceRepository.Object);
    }

    [Fact]
    public async Task GetDeviceByIdAsync_ShouldReturn_Null_WhenNotFound()
    {
        // Arrange
        _mockDeviceRepository.Setup(r => r.GetDeviceByIdAsync("2")).ReturnsAsync((Device?)null); // Alinha com o método ajustado

        // Act
        var result = await _deviceService.GetDeviceByIdAsync("2");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteDeviceAsync_ShouldThrowException_WhenDeviceNotFound()
    {
        // Arrange
        _mockDeviceRepository.Setup(r => r.GetDeviceByIdAsync("2")).ReturnsAsync((Device?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _deviceService.DeleteDeviceAsync("2"));
        Assert.Equal("Dispositivo não encontrado.", exception.Message); // Verifica a mensagem da exceção
    }
}

