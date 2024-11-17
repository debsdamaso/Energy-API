using Energy_API.Models;
using Energy_API.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Energy_API.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IMongoCollection<Device> _devices;

        public DeviceRepository(IMongoDatabase database)
        {
            _devices = database.GetCollection<Device>("Devices");
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            return await _devices.Find(_ => true).ToListAsync();
        }

        public async Task<Device?> GetDeviceByIdAsync(string id)
        {
            return await _devices.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateDeviceAsync(Device device)
        {
            await _devices.InsertOneAsync(device);
        }

        public async Task UpdateDeviceAsync(string id, Device device)
        {
            await _devices.ReplaceOneAsync(d => d.Id == id, device);
        }

        public async Task DeleteDeviceAsync(string id)
        {
            await _devices.DeleteOneAsync(d => d.Id == id);
        }
    }
}
