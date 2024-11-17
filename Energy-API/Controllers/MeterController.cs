using Energy_API.Models;
using Energy_API.Services.Interfaces; // Importando a interface
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Energy_API.Controllers
{
    [ApiController]
    [Route("api/meters")]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService; // Alterado para a interface

        public MeterController(IMeterService meterService) // Ajustado para aceitar a interface
        {
            _meterService = meterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeters()
        {
            var meters = await _meterService.GetAllMetersAsync();
            return Ok(meters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeterById(string id)
        {
            var meter = await _meterService.GetMeterByIdAsync(id);
            if (meter == null) return NotFound();
            return Ok(meter);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeter([FromBody] Meter meter)
        {
            await _meterService.CreateMeterAsync(meter);
            return CreatedAtAction(nameof(GetMeterById), new { id = meter.Id }, meter);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeter(string id, [FromBody] Meter meter)
        {
            await _meterService.UpdateMeterAsync(id, meter);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeter(string id)
        {
            await _meterService.DeleteMeterAsync(id);
            return NoContent();
        }
    }
}
