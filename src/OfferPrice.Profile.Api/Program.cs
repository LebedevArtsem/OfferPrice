using OfferPrice.Profile.Api.Filters;
using OfferPrice.Common.Extensions;
using OfferPrice.Profile.Api.Extensions;
using OfferPrice.Profile.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddGatewayAuthentication();
builder.Services.RegisterSwagger();

builder.Services.AddVersioning(config);
builder.Services.RegisterEmail(config);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.RegisterDatebase(settings.Database);

builder.Services.RegisterRabbitMq(settings.RabbitMq);

builder.Services.AddProblemDetails();
builder.Services.AddScoped<OperationCanceledFilter>();
builder.Services.AddControllers(options =>
{
    //options.Filters.Add<OperationCanceledFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.MapControllers();

await app.RunAsync();