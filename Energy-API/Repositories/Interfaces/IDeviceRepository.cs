using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Energy_API.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(string id);
        Task CreateDeviceAsync(Device device);
        Task UpdateDeviceAsync(string id, Device device);
        Task DeleteDeviceAsync(string id);
    }
}
