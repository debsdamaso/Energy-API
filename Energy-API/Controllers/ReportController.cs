using Energy_API.DTOs;
using Energy_API.Models;
using Energy_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Energy_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly DeviceService _deviceService;
        private readonly MeterService _meterService;

        public ReportController(DeviceService deviceService, MeterService meterService)
        {
            _deviceService = deviceService;
            _meterService = meterService;
        }

        [HttpGet("consumption-by-location")]
        public async Task<IActionResult> GetConsumptionByLocation()
        {
            var meters = await _meterService.GetAllMetersAsync();
            if (meters == null || !meters.Any())
            {
                return Ok(new List<ConsumptionByLocationDto>());
            }

            var devices = await _deviceService.GetAllDevicesAsync();
            if (devices == null || !devices.Any())
            {
                return Ok(new List<ConsumptionByLocationDto>());
            }

            var report = meters.Select(meter =>
            {
                var totalConsumption = devices
                    .Where(device => device.MeterId == meter.Id)
                    .Sum(device => device.MonthlyEnergyUsage);

                return new ConsumptionByLocationDto
                {
                    Location = meter.Location,
                    TotalConsumption = totalConsumption
                };
            }).ToList();

            return Ok(report);
        }

        [HttpGet("inefficient-devices")]
        public async Task<IActionResult> GetInefficientDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            if (devices == null || !devices.Any())
            {
                return Ok(new List<InefficientDeviceDto>());
            }

            var inefficientDevices = devices
                .Where(device => device.EfficiencyClass != "A" && device.EfficiencyClass != "B")
                .Select(device => new InefficientDeviceDto
                {
                    Name = device.Name,
                    EfficiencyClass = device.EfficiencyClass,
                    MonthlyEnergyUsage = device.MonthlyEnergyUsage
                }).ToList();

            return Ok(inefficientDevices);
        }

        [HttpGet("anomalous-consumption")]
        public async Task<IActionResult> GetDevicesWithAnomalousConsumption()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            if (devices == null || !devices.Any())
            {
                return Ok(new List<AnomalousConsumptionDto>());
            }

            // Limites definidos para consumo anômalo por tipo de dispositivo
            var thresholds = new Dictionary<string, double>
            {
                { "Ar Condicionado", 30.0 },
                { "Aquecedor", 20.0 }
            };

            var anomalousDevices = devices
                .Where(device => thresholds.ContainsKey(device.Type) && device.MonthlyEnergyUsage > thresholds[device.Type])
                .Select(device => new AnomalousConsumptionDto
                {
                    DeviceType = device.Type,
                    Consumption = device.MonthlyEnergyUsage
                }).ToList();

            return Ok(anomalousDevices);
        }
    }
}
