using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OfferPrice.Common.Extensions;
public static class VersioningExtensions
{
    public static void AddVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(config =>
        {
            int major = Convert.ToInt32(configuration.GetSection("ApiVersionMajor").Value);
            int minor = Convert.ToInt32(configuration.GetSection("ApiVersionMinor").Value);

            config.ReportApiVersions = true;
            config.DefaultApiVersion = ApiVersion.Default;
            config.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
    }
}

