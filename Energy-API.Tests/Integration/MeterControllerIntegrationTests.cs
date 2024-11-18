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
    public class MeterControllerIntegrationTests
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _testDatabase;
        private readonly MeterRepository _meterRepository;
        private readonly MeterService _meterService;
        private readonly MeterController _meterController;

        public MeterControllerIntegrationTests()
        {
            // Configura uma conexão para um banco de dados local de teste
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _testDatabase = _mongoClient.GetDatabase("EnergyApiTestDb");

            // Inicializa os componentes reais
            _meterRepository = new MeterRepository(_testDatabase);
            _meterService = new MeterService(_meterRepository);
            _meterController = new MeterController(_meterService);
        }

        [Fact]
        public async Task GetAllMeters_ShouldReturn_AllMeters()
        {
            // Limpar a coleção antes do teste
            var metersCollection = _testDatabase.GetCollection<Meter>("Meters");
            await metersCollection.DeleteManyAsync(FilterDefinition<Meter>.Empty);

            // Inserir dados de teste
            var meters = new List<Meter>
            {
                new Meter
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Location = "Living Room",
                    CreatedAt = DateTime.UtcNow
                },
                new Meter
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Location = "Basement",
                    CreatedAt = DateTime.UtcNow
                }
            };
            await metersCollection.InsertManyAsync(meters);

            // Act
            var result = await _meterController.GetAllMeters();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMeters = Assert.IsType<List<Meter>>(okResult.Value);
            Assert.Equal(2, returnedMeters.Count);
        }

        [Fact]
        public async Task GetMeterById_ShouldReturn_Meter_WhenExists()
        {
            // Arrange
            var metersCollection = _testDatabase.GetCollection<Meter>("Meters");
            await metersCollection.DeleteManyAsync(FilterDefinition<Meter>.Empty);

            var meter = new Meter
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Location = "Living Room",
                CreatedAt = DateTime.UtcNow
            };

            await metersCollection.InsertOneAsync(meter);

            // Act
            var result = await _meterController.GetMeterById(meter.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMeter = Assert.IsType<Meter>(okResult.Value);
            Assert.Equal(meter.Id, returnedMeter.Id);
        }

        [Fact]
        public async Task CreateMeter_ShouldAdd_NewMeter()
        {
            // Arrange
            var metersCollection = _testDatabase.GetCollection<Meter>("Meters");
            await metersCollection.DeleteManyAsync(FilterDefinition<Meter>.Empty);

            var newMeter = new Meter
            {
                Location = "Garage",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _meterController.CreateMeter(newMeter);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdMeter = Assert.IsType<Meter>(createdAtResult.Value);

            Assert.NotNull(createdMeter.Id);
            var meterInDb = await metersCollection.Find(m => m.Id == createdMeter.Id).FirstOrDefaultAsync();
            Assert.NotNull(meterInDb);
        }

        [Fact]
        public async Task UpdateMeter_ShouldModify_ExistingMeter()
        {
            // Arrange
            var metersCollection = _testDatabase.GetCollection<Meter>("Meters");
            await metersCollection.DeleteManyAsync(FilterDefinition<Meter>.Empty);

            var meter = new Meter
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Location = "Office",
                CreatedAt = DateTime.UtcNow
            };

            await metersCollection.InsertOneAsync(meter);

            var updatedMeter = new Meter
            {
                Id = meter.Id,
                Location = "Updated Office",
                CreatedAt = meter.CreatedAt
            };

            // Act
            var result = await _meterController.UpdateMeter(meter.Id, updatedMeter);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            var meterInDb = await metersCollection.Find(m => m.Id == meter.Id).FirstOrDefaultAsync();

            Assert.NotNull(meterInDb);
            Assert.Equal("Updated Office", meterInDb.Location);
        }

        [Fact]
        public async Task DeleteMeter_ShouldRemove_Meter()
        {
            // Arrange
            var metersCollection = _testDatabase.GetCollection<Meter>("Meters");
            await metersCollection.DeleteManyAsync(FilterDefinition<Meter>.Empty);

            var meter = new Meter
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Location = "Test Location",
                CreatedAt = DateTime.UtcNow
            };

            await metersCollection.InsertOneAsync(meter);

            // Act
            var result = await _meterController.DeleteMeter(meter.Id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            var meterInDb = await metersCollection.Find(m => m.Id == meter.Id).FirstOrDefaultAsync();

            Assert.Null(meterInDb);
        }
    }
}
