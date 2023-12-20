using MongoDB.Driver;
using OfferPrice.Payment.Api.Settings;
using OfferPrice.Payment.Domain.Interfaces;
using OfferPrice.Payment.Infrastructure.Repositories;
using OfferPrice.Payment.Infrastructure.Events;
using OfferPrice.Events.RabbitMq;
using OfferPrice.Events.RabbitMq.Options;

namespace OfferPrice.Payment.Api.Extensions;

public static class ServiceCollectionExtesions
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, DatabaseSettings settings)
    {
        services.AddSingleton(
            _ => new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName));

        services.AddSingleton<ITransactionRepository, TransactionRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<ILotRepository, LotRepository>();

        return services;
    }

    public static IServiceCollection RegisterRabbitMq(this IServiceCollection services, RabbitMqSettings settings)
    {
        services.AddRabbitMqProducer(settings);

        services.AddRabbitMqConsumer<UserCreatedEventConsumer>(settings);

        return services;
    }
}

