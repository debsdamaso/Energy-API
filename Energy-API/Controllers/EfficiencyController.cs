using Energy_API.DTOs;
using Energy_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Energy_API.Controllers
{
    [ApiController]
    [Route("api/efficiency")]
    public class EfficiencyController : ControllerBase
    {
        private readonly EfficiencyService _efficiencyService;

        public EfficiencyController(EfficiencyService efficiencyService)
        {
            _efficiencyService = efficiencyService;
        }

        [HttpPost("classify")]
        public IActionResult ClassifyDevice([FromBody] DeviceUsageDto dto)
        {
            string efficiencyClass = _efficiencyService.ClassifyDeviceEfficiency(dto.DeviceType, dto.MonthlyUsage);
            return Ok(new { EfficiencyClass = efficiencyClass });
        }
    }
}
