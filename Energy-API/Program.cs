using Energy_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do MongoDB
builder.Services.AddSingleton(provider =>
{
    var configuration = builder.Configuration;
    var mongoConfig = new Energy_API.Config.MongoDBConfig(configuration);
    return mongoConfig.GetDatabase();
});

// Adicionando Reposit�rios
builder.Services.AddScoped<Energy_API.Repositories.Interfaces.IDeviceRepository, Energy_API.Repositories.DeviceRepository>();
builder.Services.AddScoped<Energy_API.Repositories.Interfaces.IMeterRepository, Energy_API.Repositories.MeterRepository>();

// Adicionando Servi�os
builder.Services.AddScoped<Energy_API.Services.DeviceService>();
builder.Services.AddScoped<Energy_API.Services.MeterService>();
builder.Services.AddScoped<Energy_API.Services.EfficiencyService>();

// Adicionando Controllers
builder.Services.AddControllers();

// Configurando Swagger para Documenta��o
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Energy-API",
        Version = "v1",
        Description = "API para gerenciamento de dispositivos e medidores com an�lise de efici�ncia energ�tica."
    });
});

var app = builder.Build();

// Configurando o Swagger (apenas no ambiente de desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
