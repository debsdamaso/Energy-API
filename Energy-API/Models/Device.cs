using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Energy_API.Models
{
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty; // Ex.: "Ar Condicionado", "Fogão"

        [BsonElement("currentWatts")]
        public double CurrentWatts { get; set; } // Potência atual em watts

        [BsonElement("estimatedUsageHours")]
        public double EstimatedUsageHours { get; set; } // Horas estimadas de uso por dia

        [BsonElement("monthlyEnergyUsage")]
        public double MonthlyEnergyUsage { get; set; } // Consumo mensal estimado em kWh

        [BsonElement("efficiencyClass")]
        public string EfficiencyClass { get; set; } = string.Empty; // Ex.: "A", "B", "C"

        [BsonElement("meterId")]
        public string MeterId { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
