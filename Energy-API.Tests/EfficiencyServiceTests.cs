using Xunit;
using Energy_API.Services;

public class EfficiencyServiceTests
{
    private readonly EfficiencyService _efficiencyService;

    public EfficiencyServiceTests()
    {
        _efficiencyService = new EfficiencyService();
    }

    [Fact]
    public void ClassifyDeviceEfficiency_ShouldReturn_A_ForLowUsage()
    {
        // Arrange
        string deviceType = "Ar Condicionado";
        double monthlyUsage = 50;

        // Act
        var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

        // Assert
        Assert.Equal("A", result);
    }

    [Fact]
    public void ClassifyDeviceEfficiency_ShouldReturn_B_ForModerateUsage()
    {
        // Arrange
        string deviceType = "Ar Condicionado";
        double monthlyUsage = 150;

        // Act
        var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

        // Assert
        Assert.Equal("B", result);
    }

    [Fact]
    public void ClassifyDeviceEfficiency_ShouldReturn_D_ForHighUsage()
    {
        // Arrange
        string deviceType = "Ar Condicionado";
        double monthlyUsage = 400;

        // Act
        var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

        // Assert
        Assert.Equal("D", result);
    }

    [Fact]
    public void ClassifyDeviceEfficiency_ShouldReturn_A_ForDefaultDeviceWithLowUsage()
    {
        // Arrange
        string deviceType = "Outro";
        double monthlyUsage = 30;

        // Act
        var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

        // Assert
        Assert.Equal("A", result);
    }

    [Fact]
    public void ClassifyDeviceEfficiency_ShouldReturn_C_ForDefaultDeviceWithModerateUsage()
    {
        // Arrange
        string deviceType = "Outro";
        double monthlyUsage = 150;

        // Act
        var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

        // Assert
        Assert.Equal("C", result);
    }

    [Theory]
    [InlineData("Ar Condicionado", 50, "A")]
    [InlineData("Fogão", 15, "A")]
    [InlineData("Micro-ondas", 75, "B")]
    [InlineData("Forno elétrico", 120, "B")]
    [InlineData("Lâmpada", 8, "B")]
    [InlineData("Lavador de roupa", 170, "C")]
    [InlineData("Refrigerador", 160, "D")]
    [InlineData("Televisor", 65, "B")]
    [InlineData("Ventilador", 25, "B")]
    public void ClassifyDeviceEfficiency_ShouldReturn_CorrectClassification(
        string deviceType, double monthlyUsage, string expectedClass)
    {
        // Act
        var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

        // Assert
        Assert.Equal(expectedClass, result);
    }
}
