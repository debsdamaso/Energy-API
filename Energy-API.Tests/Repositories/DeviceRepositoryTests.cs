using Xunit;
using Moq;
using MongoDB.Driver;
using Energy_API.Models;
using Energy_API.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Energy_API.Tests.Repositories
{
    public class DeviceRepositoryTests
    {
        private readonly Mock<IMongoCollection<Device>> _mockCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly DeviceRepository _deviceRepository;

        public DeviceRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Device>>();
            _mockDatabase = new Mock<IMongoDatabase>();

            _mockDatabase
                .Setup(db => db.GetCollection<Device>(It.IsAny<string>(), null))
                .Returns(_mockCollection.Object);

            _deviceRepository = new DeviceRepository(_mockDatabase.Object);
        }

        [Fact]
        public async Task CreateDevice_ShouldCall_InsertOneAsync()
        {
            // Arrange
            var device = new Device { Id = "1", Name = "Smart TV" };

            // Act
            await _deviceRepository.CreateDeviceAsync(device);

            // Assert
            _mockCollection.Verify(
                col => col.InsertOneAsync(
                    device,
                    null,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllDevices_ShouldReturn_AllDevices()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device { Id = "1", Name = "Smart TV" }
            };

            var asyncCursorMock = new Mock<IAsyncCursor<Device>>();
            asyncCursorMock
                .Setup(cursor => cursor.Current)
                .Returns(devices);
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true) // Simula que há dados na primeira chamada
                .Returns(false); // Simula o fim dos dados
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true) // Simula que há dados
                .ReturnsAsync(false); // Simula o fim dos dados

            _mockCollection
                .Setup(col => col.FindAsync(
                    It.IsAny<FilterDefinition<Device>>(),
                    It.IsAny<FindOptions<Device, Device>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _deviceRepository.GetAllDevicesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Smart TV", result[0].Name);
        }

        [Fact]
        public async Task GetDeviceById_ShouldReturn_Device_WhenExists()
        {
            // Arrange
            var device = new Device { Id = "1", Name = "Smartphone" };

            var asyncCursorMock = new Mock<IAsyncCursor<Device>>();
            asyncCursorMock
                .Setup(cursor => cursor.Current)
                .Returns(new List<Device> { device });
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection
                .Setup(col => col.FindAsync(
                    It.IsAny<FilterDefinition<Device>>(),
                    It.IsAny<FindOptions<Device, Device>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _deviceRepository.GetDeviceByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Smartphone", result?.Name);
        }

        [Fact]
        public async Task GetDeviceById_ShouldReturn_Null_WhenDoesNotExist()
        {
            // Arrange
            var asyncCursorMock = new Mock<IAsyncCursor<Device>>();
            asyncCursorMock
                .Setup(cursor => cursor.Current)
                .Returns(new List<Device>());
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(false);
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockCollection
                .Setup(col => col.FindAsync(
                    It.IsAny<FilterDefinition<Device>>(),
                    It.IsAny<FindOptions<Device, Device>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _deviceRepository.GetDeviceByIdAsync("999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteDevice_ShouldCall_DeleteOneAsync()
        {
            // Act
            await _deviceRepository.DeleteDeviceAsync("1");

            // Assert
            _mockCollection.Verify(
                col => col.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Device>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
