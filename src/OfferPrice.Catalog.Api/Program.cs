using OfferPrice.Catalog.Api.Extensions;
using OfferPrice.Catalog.Api.Filters;
using OfferPrice.Catalog.Api.Settings;
using OfferPrice.Common.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var settings = builder.Configuration.Get<AppSettings>()!;

builder.Services.AddSingleton(settings);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddGatewayAuthentication();
builder.Services.RegisterSwagger();
builder.Services.AddVersioning(config);

builder.Services.RegisterInfrastructure(settings.Database);
builder.Services.RegisterRabbitMq(settings.RabbitMq);

builder.Services.AddProblemDetails();

builder.Services.AddScoped<OperationCanceledFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<OperationCanceledFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
