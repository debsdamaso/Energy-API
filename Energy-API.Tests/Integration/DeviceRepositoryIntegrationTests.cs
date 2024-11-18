using Xunit;
using MongoDB.Driver;
using Energy_API.Models;
using Energy_API.Repositories;

namespace Energy_API.Tests.Integration
{
    public class DeviceRepositoryIntegrationTests
    {
        private readonly IMongoCollection<Device> _deviceCollection;
        private readonly DeviceRepository _deviceRepository;

        public DeviceRepositoryIntegrationTests()
        {
            // Configuração do cliente MongoDB
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TestDatabase");

            // Configurando a coleção para teste
            _deviceCollection = database.GetCollection<Device>("Devices");
            _deviceRepository = new DeviceRepository(database);

            // Limpeza inicial
            _deviceCollection.DeleteMany(Builders<Device>.Filter.Empty);
        }

        [Fact]
        public async void CreateDevice_ShouldPersistDevice()
        {
            // Arrange
            var device = new Device
            {
                Name = "Test Device",
                Type = "Test Type",
                CurrentWatts = 100,
                MonthlyEnergyUsage = 50
            };

            // Act
            await _deviceRepository.CreateDeviceAsync(device);

            // Assert
            var persistedDevice = await _deviceCollection.Find(d => d.Name == "Test Device").FirstOrDefaultAsync();
            Assert.NotNull(persistedDevice);
            Assert.Equal("Test Type", persistedDevice.Type);
        }

        [Fact]
        public async void GetAllDevices_ShouldReturnPersistedDevices()
        {
            // Arrange
            var device1 = new Device { Name = "Device1", Type = "Type1", CurrentWatts = 200 };
            var device2 = new Device { Name = "Device2", Type = "Type2", CurrentWatts = 150 };

            await _deviceRepository.CreateDeviceAsync(device1);
            await _deviceRepository.CreateDeviceAsync(device2);

            // Act
            var devices = await _deviceRepository.GetAllDevicesAsync();

            // Assert
            Assert.Equal(2, devices.Count);
        }
    }
}
