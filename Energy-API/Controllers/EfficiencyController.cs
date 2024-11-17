using Energy_API.DTOs;
using Energy_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Energy_API.Controllers
{
    [ApiController]
    [Route("api/efficiency")]
    public class EfficiencyController : ControllerBase
    {
        private readonly IEfficiencyService _efficiencyService;

        public EfficiencyController(IEfficiencyService efficiencyService)
        {
            _efficiencyService = efficiencyService;
        }

        [HttpPost("classify")]
        public IActionResult ClassifyDevice([FromBody] DeviceUsageDto dto)
        {
            string efficiencyClass = _efficiencyService.ClassifyDeviceEfficiency(dto.DeviceType, dto.MonthlyUsage);
            return Ok(new EfficiencyResponseDto { EfficiencyClass = efficiencyClass });
        }
    }
}
