using Microsoft.OpenApi.Models;
using MicrosoftStoreBadge.Services;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;

services.AddControllers().AddJsonOptions(config =>
{
    config.JsonSerializerOptions.Converters.Add(new JsonStringEnumMemberConverter(JsonNamingPolicy.CamelCase));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo());
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
services.AddOutputCache(config =>
{
    config.AddPolicy("Expire12Hours", builder => builder.Expire(TimeSpan.FromHours(12)).SetVaryByQuery("*"));
});

services.AddScoped<MicrosoftStoreService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
