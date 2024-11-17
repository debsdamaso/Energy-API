using Energy_API.Models;
using Energy_API.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Energy_API.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllDevicesAsync();
            return devices ?? new List<Device>(); // Garante que nunca retorna nulo
        }

        public async Task<Device?> GetDeviceByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("O ID do dispositivo não pode ser nulo ou vazio.", nameof(id));

            return await _deviceRepository.GetDeviceByIdAsync(id);
        }

        public async Task CreateDeviceAsync(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "O dispositivo não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(device.Name))
                throw new ArgumentException("O nome do dispositivo é obrigatório.", nameof(device.Name));

            await _deviceRepository.CreateDeviceAsync(device);
        }

        public async Task UpdateDeviceAsync(string id, Device device)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("O ID do dispositivo não pode ser nulo ou vazio.", nameof(id));

            if (device == null)
                throw new ArgumentNullException(nameof(device), "O dispositivo não pode ser nulo.");

            var existingDevice = await _deviceRepository.GetDeviceByIdAsync(id);

            if (existingDevice == null)
                throw new KeyNotFoundException("Dispositivo não encontrado.");

            await _deviceRepository.UpdateDeviceAsync(id, device);
        }

        public async Task DeleteDeviceAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("O ID do dispositivo não pode ser nulo ou vazio.", nameof(id));

            var existingDevice = await _deviceRepository.GetDeviceByIdAsync(id);

            if (existingDevice == null)
                throw new KeyNotFoundException("Dispositivo não encontrado.");

            await _deviceRepository.DeleteDeviceAsync(id);
        }
    }
}
