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
    public class MeterRepositoryTests
    {
        private readonly Mock<IMongoCollection<Meter>> _mockCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly MeterRepository _meterRepository;

        public MeterRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Meter>>();
            _mockDatabase = new Mock<IMongoDatabase>();

            _mockDatabase
                .Setup(db => db.GetCollection<Meter>(It.IsAny<string>(), null))
                .Returns(_mockCollection.Object);

            _meterRepository = new MeterRepository(_mockDatabase.Object);
        }

        [Fact]
        public async Task CreateMeter_ShouldCall_InsertOneAsync()
        {
            // Arrange
            var meter = new Meter { Id = "1", Location = "Living Room" };

            // Act
            await _meterRepository.CreateMeterAsync(meter);

            // Assert
            _mockCollection.Verify(
                col => col.InsertOneAsync(
                    meter,
                    null,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllMeters_ShouldReturn_AllMeters()
        {
            // Arrange
            var meters = new List<Meter>
            {
                new Meter { Id = "1", Location = "Living Room" }
            };

            var asyncCursorMock = new Mock<IAsyncCursor<Meter>>();
            asyncCursorMock
                .Setup(cursor => cursor.Current)
                .Returns(meters);
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
                    It.IsAny<FilterDefinition<Meter>>(),
                    It.IsAny<FindOptions<Meter, Meter>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _meterRepository.GetAllMetersAsync();

            // Assert
            Assert.Single(result); // Valida que há apenas um elemento na lista
            Assert.Equal("Living Room", result[0].Location); // Valida os dados do medidor
        }

        [Fact]
        public async Task GetMeterById_ShouldReturn_Meter_WhenExists()
        {
            // Arrange
            var meter = new Meter { Id = "1", Location = "Kitchen" };

            var asyncCursorMock = new Mock<IAsyncCursor<Meter>>();
            asyncCursorMock
                .Setup(cursor => cursor.Current)
                .Returns(new List<Meter> { meter });
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true) // Simula que há dados
                .Returns(false); // Simula o fim dos dados
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true) // Simula que há dados
                .ReturnsAsync(false); // Simula o fim dos dados

            _mockCollection
                .Setup(col => col.FindAsync(
                    It.IsAny<FilterDefinition<Meter>>(),
                    It.IsAny<FindOptions<Meter, Meter>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _meterRepository.GetMeterByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Kitchen", result?.Location); // Valida os dados do medidor
        }

        [Fact]
        public async Task GetMeterById_ShouldReturn_Null_WhenDoesNotExist()
        {
            // Arrange
            var asyncCursorMock = new Mock<IAsyncCursor<Meter>>();
            asyncCursorMock
                .Setup(cursor => cursor.Current)
                .Returns(new List<Meter>());
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(false); // Simula que não há dados
            asyncCursorMock
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); // Simula que não há dados

            _mockCollection
                .Setup(col => col.FindAsync(
                    It.IsAny<FilterDefinition<Meter>>(),
                    It.IsAny<FindOptions<Meter, Meter>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _meterRepository.GetMeterByIdAsync("999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteMeter_ShouldCall_DeleteOneAsync()
        {
            // Act
            await _meterRepository.DeleteMeterAsync("1");

            // Assert
            _mockCollection.Verify(
                col => col.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Meter>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
