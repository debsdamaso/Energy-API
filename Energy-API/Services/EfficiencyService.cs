using Energy_API.Models;
using Energy_API.Services.Interfaces;
using System;

namespace Energy_API.Services
{
    public class EfficiencyService : IEfficiencyService
    {
        public string ClassifyDeviceEfficiency(string deviceType, double monthlyUsage)
        {
            if (string.IsNullOrWhiteSpace(deviceType))
                throw new ArgumentException("O tipo do dispositivo não pode ser nulo ou vazio.");

            if (monthlyUsage < 0)
                throw new ArgumentException("O consumo mensal deve ser maior ou igual a zero.");

            switch (deviceType.ToLower())
            {
                case "ar condicionado":
                    if (monthlyUsage <= 100) return "A";
                    if (monthlyUsage <= 200) return "B";
                    if (monthlyUsage <= 300) return "C";
                    return "D";

                case "fogão":
                    if (monthlyUsage <= 20) return "A";
                    if (monthlyUsage <= 40) return "B";
                    if (monthlyUsage <= 60) return "C"; // Inclui consumo 50 como "C"
                    return "D";

                case "micro-ondas":
                    if (monthlyUsage <= 30) return "A";
                    if (monthlyUsage <= 75) return "B";
                    if (monthlyUsage <= 90) return "C";
                    return "D";

                case "forno elétrico":
                    if (monthlyUsage <= 50) return "A";
                    if (monthlyUsage <= 120) return "B";
                    if (monthlyUsage <= 150) return "C";
                    return "D";

                case "lâmpada":
                    if (monthlyUsage <= 5) return "A";
                    if (monthlyUsage <= 10) return "B";
                    if (monthlyUsage <= 15) return "C";
                    return "D";

                case "lavador de roupa":
                    if (monthlyUsage <= 80) return "A";
                    if (monthlyUsage <= 150) return "B";
                    if (monthlyUsage <= 200) return "C";
                    return "D";

                case "refrigerador":
                    if (monthlyUsage <= 50) return "A";
                    if (monthlyUsage <= 100) return "B";
                    if (monthlyUsage <= 150) return "C";
                    return "D";

                case "televisor":
                    if (monthlyUsage <= 30) return "A";
                    if (monthlyUsage <= 65) return "B";
                    if (monthlyUsage <= 90) return "C";
                    return "D";

                case "ventilador":
                    if (monthlyUsage <= 15) return "A";
                    if (monthlyUsage <= 30) return "B";
                    if (monthlyUsage <= 50) return "C";
                    return "D";

                default:
                    // Regra genérica para dispositivos desconhecidos
                    if (monthlyUsage <= 50) return "A";
                    if (monthlyUsage <= 100) return "B";
                    if (monthlyUsage <= 200) return "C";
                    return "D";
            }
        }
    }
}
