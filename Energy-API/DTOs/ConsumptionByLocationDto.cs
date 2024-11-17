using System;

namespace Energy_API.DTOs
{
    public class ConsumptionByLocationDto
    {
        public string Location { get; set; }
        public double TotalConsumption { get; set; }
    }
}
