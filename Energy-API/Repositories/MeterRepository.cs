using Energy_API.Models;
using Energy_API.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Energy_API.Repositories
{
    public class MeterRepository : IMeterRepository
    {
        private readonly IMongoCollection<Meter> _meters;

        public MeterRepository(IMongoDatabase database)
        {
            _meters = database.GetCollection<Meter>("Meters");
        }

        public async Task<List<Meter>> GetAllMetersAsync()
        {
            return await _meters.Find(_ => true).ToListAsync();
        }

        public async Task<Meter?> GetMeterByIdAsync(string id)
        {
            return await _meters.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateMeterAsync(Meter meter)
        {
            await _meters.InsertOneAsync(meter);
        }

        public async Task UpdateMeterAsync(string id, Meter meter)
        {
            await _meters.ReplaceOneAsync(m => m.Id == id, meter);
        }

        public async Task DeleteMeterAsync(string id)
        {
            await _meters.DeleteOneAsync(m => m.Id == id);
        }
    }
}
