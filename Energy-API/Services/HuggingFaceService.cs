using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Energy_API.Services
{
    public class HuggingFaceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public HuggingFaceService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = configuration["HuggingFace:ApiKey"];
            _model = configuration["HuggingFace:Model"];
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            var url = $"https://api-inference.huggingface.co/models/{_model}";

            // Corpo da requisição com o prompt e parâmetros avançados
            var requestBody = new
            {
                inputs = prompt,  // O prompt é enviado aqui
                parameters = new
                {
                    max_length = 150, // Limita o número de tokens na resposta
                    temperature = 0.7, // Controla a aleatoriedade da resposta
                    top_p = 0.9 // Probabilidade cumulativa para selecionar tokens
                }
            };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            // Enviar requisição
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.PostAsync(url, requestContent);
                response.EnsureSuccessStatusCode();  // Garantir que o código de status seja 200-299
            }
            catch (HttpRequestException ex)
            {
                return $"Erro ao fazer a requisição: {ex.Message}";
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                    // Verifica se a resposta contém "generated_text"
                    if (responseObject.ValueKind == JsonValueKind.Array && responseObject.GetArrayLength() > 0)
                    {
                        return responseObject[0].GetProperty("generated_text").GetString();
                    }
                    return "Resposta inesperada da API. Verifique o formato do modelo.";
                }
                catch (JsonException jsonEx)
                {
                    return $"Erro ao processar a resposta JSON: {jsonEx.Message}";
                }
            }

            // Retorna erro de status HTTP se não for sucesso
            return $"Erro: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
        }
    }
}
