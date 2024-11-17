using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Energy_API.Models
{
    public class AnalysisResult
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("deviceId")]
        public string DeviceId { get; set; } // ID do dispositivo analisado

        [BsonElement("expectedUsage")]
        public double ExpectedUsage { get; set; } // Consumo esperado em kWh

        [BsonElement("actualUsage")]
        public double ActualUsage { get; set; } // Consumo real em kWh

        [BsonElement("analysis")]
        public string Analysis { get; set; } // Resultado da análise, ex.: "Consumo 20% acima do esperado"

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
