using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Middleware;
using OfferPrice.Apigateway.Extensions;
using OfferPrice.Apigateway.Filters;
using OfferPrice.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.AddOcelotFilesWithSwaggerSupport();
builder.Services.AddOcelotWithSwaggerSupport(config);

builder.Services.AddJwtAuthentication(config);

builder.Services.AddVersioning(config);

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ExceptionFilter>();
builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddRouting();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();

app.UseRouting();

app.UseAuthentication();
//app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.UseSwaggerForOcelotUI();

await app.UseOcelot();

await app.RunAsync();
