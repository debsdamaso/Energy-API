using System;

namespace Energy_API.DTOs
{
    public class InefficientDeviceDto
    {
        public string Name { get; set; }
        public string EfficiencyClass { get; set; }
        public double MonthlyEnergyUsage { get; set; }
    }
}
