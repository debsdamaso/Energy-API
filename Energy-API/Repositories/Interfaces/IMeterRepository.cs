using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Energy_API.Repositories.Interfaces
{
    public interface IMeterRepository
    {
        Task<List<Meter>> GetAllMetersAsync();
        Task<Meter?> GetMeterByIdAsync(string id);
        Task CreateMeterAsync(Meter meter);
        Task UpdateMeterAsync(string id, Meter meter);
        Task DeleteMeterAsync(string id);
    }
}
