using Xunit;
using Energy_API.Services;
using Energy_API.Services.Interfaces;

namespace Energy_API.Tests.Integration
{
    public class EfficiencyServiceIntegrationTests
    {
        private readonly EfficiencyService _efficiencyService;

        public EfficiencyServiceIntegrationTests()
        {
            _efficiencyService = new EfficiencyService();
        }

        [Fact]
        public void ClassifyDeviceEfficiency_ShouldClassify_GenericForUnknownDeviceType()
        {
            // Arrange
            var deviceType = "Unknown Device";
            var monthlyUsage = 75;

            // Act
            var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

            // Assert
            Assert.Equal("B", result);
        }

        [Theory]
        [InlineData("Ar Condicionado", 150, "B")]
        [InlineData("Fogão", 50, "C")]
        [InlineData("Micro-ondas", 75, "B")]
        [InlineData("Forno elétrico", 200, "D")]
        public void ClassifyDeviceEfficiency_ShouldReturn_CorrectEfficiencyClass(string deviceType, double monthlyUsage, string expectedClass)
        {
            // Act
            var result = _efficiencyService.ClassifyDeviceEfficiency(deviceType, monthlyUsage);

            // Assert
            Assert.Equal(expectedClass, result);
        }
    }
}
