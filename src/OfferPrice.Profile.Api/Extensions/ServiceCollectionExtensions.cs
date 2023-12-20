using MongoDB.Driver;
using OfferPrice.Events.RabbitMq;
using OfferPrice.Profile.Api.Settings;
using OfferPrice.Events.RabbitMq.Options;
using OfferPrice.Profile.Domain.Interfaces;
using OfferPrice.Profile.Infrastructure.Repositories;

namespace OfferPrice.Profile.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDatebase(this IServiceCollection services, DatabaseSettings settings)
    {
        services.AddSingleton(
            _ => new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IDoubleAuthRepository, DoubleAuthRepository>();

        return services;
    }

    public static void RegisterRabbitMq(this IServiceCollection services, RabbitMqSettings settings)
    {
        services.AddRabbitMqProducer(settings);
    }
}

