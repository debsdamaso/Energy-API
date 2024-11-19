using Energy_API.Config;  // Importando o namespace correto
using Energy_API.Services;
using Energy_API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Lendo a string de conexão do MongoDB do arquivo appsettings.json
var mongoConnectionString = builder.Configuration.GetSection("MongoDB")["ConnectionString"];

if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new ArgumentNullException("MongoDB connection string is not configured or is null.");
}

// Configuração do MongoDB
builder.Services.AddSingleton(provider =>
{
    var mongoConfig = new Energy_API.Config.MongoDBConfig(builder.Configuration);
    return mongoConfig.GetDatabase();
});

// Adicionando Repositórios
builder.Services.AddScoped<Energy_API.Repositories.Interfaces.IDeviceRepository, Energy_API.Repositories.DeviceRepository>();
builder.Services.AddScoped<Energy_API.Repositories.Interfaces.IMeterRepository, Energy_API.Repositories.MeterRepository>();

// Adicionando Serviços
builder.Services.AddScoped<Energy_API.Services.DeviceService>();
builder.Services.AddScoped<Energy_API.Services.MeterService>();
builder.Services.AddScoped<Energy_API.Services.EfficiencyService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IEfficiencyService, EfficiencyService>();
builder.Services.AddScoped<IMeterService, MeterService>();
// Serviço de IA
builder.Services.AddHttpClient<HuggingFaceService>();

// Adicionando Controllers
builder.Services.AddControllers();

// Configuração do HealthCheck
builder.Services.ConfigureHealthChecks(mongoConnectionString);  // Passando a string de conexão corretamente

// Configurando Swagger para Documentação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Energy-API",
        Version = "v1",
        Description = "API para gerenciamento de dispositivos e medidores com análise de eficiência energética."
    });
});

var app = builder.Build();

// Configurando o Swagger (apenas no ambiente de desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ordem correta dos middlewares

app.UseHttpsRedirection();

// Middleware de roteamento
app.UseRouting();  // Roteamento deve vir antes de UseEndpoints()

app.UseAuthorization();

// Configuração do HealthCheck
app.MapHealthChecks("/api/health", HealthCheckConfig.JsonResponseWriter());

app.MapControllers();

app.Run();
