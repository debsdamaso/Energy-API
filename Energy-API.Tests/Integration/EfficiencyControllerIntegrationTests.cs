using Xunit;
using Energy_API.Controllers;
using Energy_API.Services;
using Energy_API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Energy_API.Tests.Integration
{
    public class EfficiencyControllerIntegrationTests
    {
        private readonly EfficiencyService _efficiencyService;
        private readonly EfficiencyController _efficiencyController;

        public EfficiencyControllerIntegrationTests()
        {
            // Inicializa o serviço real
            _efficiencyService = new EfficiencyService();
            _efficiencyController = new EfficiencyController(_efficiencyService);
        }

        [Fact]
        public void ClassifyDevice_ShouldReturn_CorrectEfficiencyClass()
        {
            // Arrange
            var dto = new DeviceUsageDto
            {
                DeviceType = "Ar Condicionado",
                MonthlyUsage = 150 // Consumo médio para classificação B
            };

            // Act
            var result = _efficiencyController.ClassifyDevice(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<EfficiencyResponseDto>(okResult.Value);
            Assert.Equal("B", response.EfficiencyClass);
        }

        [Theory]
        [InlineData("Ar Condicionado", 80, "A")]
        [InlineData("Fogão", 50, "C")]
        [InlineData("Micro-ondas", 75, "B")]
        [InlineData("Forno elétrico", 200, "D")]
        public void ClassifyDevice_ShouldHandle_MultipleScenarios(string deviceType, double monthlyUsage, string expectedClass)
        {
            // Arrange
            var dto = new DeviceUsageDto
            {
                DeviceType = deviceType,
                MonthlyUsage = monthlyUsage
            };

            // Act
            var result = _efficiencyController.ClassifyDevice(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<EfficiencyResponseDto>(okResult.Value);
            Assert.Equal(expectedClass, response.EfficiencyClass);
        }
    }
}
