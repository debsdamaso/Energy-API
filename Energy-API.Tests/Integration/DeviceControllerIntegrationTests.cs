using Xunit;
using MongoDB.Driver;
using Energy_API.Controllers;
using Energy_API.Repositories;
using Energy_API.Services;
using Energy_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Energy_API.Tests.Integration
{
    public class DeviceControllerIntegrationTests
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _testDatabase;
        private readonly DeviceRepository _deviceRepository;
        private readonly DeviceService _deviceService;
        private readonly DeviceController _deviceController;

        public DeviceControllerIntegrationTests()
        {
            // Configura uma conexão para um banco de dados local de teste
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _testDatabase = _mongoClient.GetDatabase("EnergyApiTestDb");

            // Inicializa os componentes reais
            _deviceRepository = new DeviceRepository(_testDatabase);
            _deviceService = new DeviceService(_deviceRepository);
            _deviceController = new DeviceController(_deviceService);
        }

        [Fact]
        public async Task GetAllDevices_ShouldReturn_AllDevices()
        {
            // Limpar a coleção antes do teste
            var devicesCollection = _testDatabase.GetCollection<Device>("Devices");
            await devicesCollection.DeleteManyAsync(FilterDefinition<Device>.Empty);

            // Inserir dados de teste
            var devices = new List<Device>
            {
                new Device
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Smart TV",
                    Type = "Electronics",
                    CurrentWatts = 100,
                    EstimatedUsageHours = 4,
                    MonthlyEnergyUsage = 12,
                    EfficiencyClass = "A",
                    MeterId = ObjectId.GenerateNewId().ToString(),
                    CreatedAt = DateTime.UtcNow
                },
                new Device
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Refrigerator",
                    Type = "Appliance",
                    CurrentWatts = 200,
                    EstimatedUsageHours = 24,
                    MonthlyEnergyUsage = 144,
                    EfficiencyClass = "B",
                    MeterId = ObjectId.GenerateNewId().ToString(),
                    CreatedAt = DateTime.UtcNow
                }
            };
            await devicesCollection.InsertManyAsync(devices);

            // Act
            var result = await _deviceController.GetAllDevices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDevices = Assert.IsType<List<Device>>(okResult.Value);
            Assert.Equal(2, returnedDevices.Count);
        }
    }
}