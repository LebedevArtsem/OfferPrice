using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfferPrice.Common.Email;
using OfferPrice.Common.Email.Options;

namespace OfferPrice.Common.Extensions;
public static class EmailExtension
{
    public static IServiceCollection RegisterEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        services.AddScoped<IEmailProviderService, EmailProvicerService>();

        return services;
    }
}

