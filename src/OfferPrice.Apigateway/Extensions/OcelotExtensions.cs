using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using OfferPrice.Apigateway.OcelotHandlers;

namespace OfferPrice.Apigateway.Extensions;

public static class OcelotExtensions
{
    public static void AddOcelotFilesWithSwaggerSupport(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            var environment = context.HostingEnvironment.EnvironmentName;

            config.AddOcelotWithSwaggerSupport(options =>
            {
                options.Folder = $"./OcelotConfig/{environment}";
                options.FileOfSwaggerEndPoints = $"ocelot.{environment}.SwaggerEndpoints";
            });
        });
    }

    public static void AddOcelotWithSwaggerSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOcelot(configuration)
                .AddDelegatingHandler<AuthorizationHandler>();
        services.AddSwaggerForOcelot(configuration);
    }
}

