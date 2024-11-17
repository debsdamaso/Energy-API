using System;

namespace Energy_API.DTOs
{
    public class AnomalousConsumptionDto
    {
        public string DeviceType { get; set; }
        public double Consumption { get; set; }
    }
}
