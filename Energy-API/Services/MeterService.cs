using Energy_API.Models;
using Energy_API.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Energy_API.Services
{
    public class MeterService
    {
        private readonly IMeterRepository _meterRepository;

        public MeterService(IMeterRepository meterRepository)
        {
            _meterRepository = meterRepository ?? throw new ArgumentNullException(nameof(meterRepository));
        }

        public async Task<List<Meter>> GetAllMetersAsync()
        {
            var meters = await _meterRepository.GetAllMetersAsync();
            return meters ?? new List<Meter>(); // Garante que nunca retorna nulo
        }

        public async Task<Meter?> GetMeterByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("O ID do medidor não pode ser nulo ou vazio.", nameof(id));

            return await _meterRepository.GetMeterByIdAsync(id);
        }

        public async Task CreateMeterAsync(Meter meter)
        {
            if (meter == null)
                throw new ArgumentNullException(nameof(meter), "O medidor não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(meter.Location))
                throw new ArgumentException("A localização do medidor é obrigatória.", nameof(meter.Location));

            await _meterRepository.CreateMeterAsync(meter);
        }

        public async Task UpdateMeterAsync(string id, Meter meter)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("O ID do medidor não pode ser nulo ou vazio.", nameof(id));

            if (meter == null)
                throw new ArgumentNullException(nameof(meter), "O medidor não pode ser nulo.");

            var existingMeter = await _meterRepository.GetMeterByIdAsync(id);

            if (existingMeter == null)
                throw new KeyNotFoundException("Medidor não encontrado.");

            await _meterRepository.UpdateMeterAsync(id, meter);
        }

        public async Task DeleteMeterAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("O ID do medidor não pode ser nulo ou vazio.", nameof(id));

            var existingMeter = await _meterRepository.GetMeterByIdAsync(id);

            if (existingMeter == null)
                throw new KeyNotFoundException("Medidor não encontrado.");

            await _meterRepository.DeleteMeterAsync(id);
        }
    }
}
