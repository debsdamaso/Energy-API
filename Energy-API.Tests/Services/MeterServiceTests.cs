using Xunit;
using Moq;
using Energy_API.Services;
using Energy_API.Repositories.Interfaces;
using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MeterServiceTests
{
    private readonly Mock<IMeterRepository> _mockMeterRepository;
    private readonly MeterService _meterService;

    public MeterServiceTests()
    {
        _mockMeterRepository = new Mock<IMeterRepository>();
        _meterService = new MeterService(_mockMeterRepository.Object);
    }

    [Fact]
    public async Task GetMeterByIdAsync_ShouldReturn_Null_WhenNotFound()
    {
        // Arrange
        _mockMeterRepository.Setup(r => r.GetMeterByIdAsync("2")).ReturnsAsync((Meter?)null); // Alinha com o método ajustado

        // Act
        var result = await _meterService.GetMeterByIdAsync("2");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteMeterAsync_ShouldThrowException_WhenMeterNotFound()
    {
        // Arrange
        _mockMeterRepository.Setup(r => r.GetMeterByIdAsync("2")).ReturnsAsync((Meter?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _meterService.DeleteMeterAsync("2"));
        Assert.Equal("Medidor não encontrado.", exception.Message); // Verifica a mensagem da exceção
    }
}
