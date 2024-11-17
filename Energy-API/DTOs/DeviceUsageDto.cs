namespace Energy_API.DTOs
{
    public class DeviceUsageDto
    {
        public string DeviceType { get; set; } // Tipo do dispositivo, ex.: "Ar Condicionado"
        public double MonthlyUsage { get; set; } // Consumo mensal estimado em kWh
    }
}
