using OfferPrice.Common.Extensions;
using OfferPrice.Payment.Api.Extensions;
using OfferPrice.Payment.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var settings = config.Get<AppSettings>();

builder.Services.RegisterDatabase(settings.Database);

builder.Services.AddGatewayAuthentication();
builder.Services.RegisterSwagger();
builder.Services.AddVersioning(config);

builder.Services.RegisterRabbitMq(settings.RabbitMq);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
