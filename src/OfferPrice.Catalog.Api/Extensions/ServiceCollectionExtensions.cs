using MongoDB.Driver;
using OfferPrice.Catalog.Api.Settings;
using OfferPrice.Catalog.Domain;
using OfferPrice.Catalog.Infrastructure.Events;
using OfferPrice.Catalog.Infrastructure.Repositories;
using OfferPrice.Events.RabbitMq;
using OfferPrice.Events.RabbitMq.Options;

namespace OfferPrice.Catalog.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRabbitMq(this IServiceCollection services, RabbitMqSettings settings)
    {
        services.AddRabbitMqProducer(settings);

        services.AddRabbitMqConsumer<LotStatusUpdatedEventConsumer>(settings);

        return services;
    }

    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, DatabaseSettings settings)
    {
        services.AddSingleton(
            _ => new MongoClient(settings.ConnectionString).GetDatabase(settings.Name));

        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<ILikeRepository, LikeRepository>();

        return services;
    }
}

