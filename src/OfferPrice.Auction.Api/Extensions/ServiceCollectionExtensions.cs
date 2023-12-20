using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using OfferPrice.Auction.Api.Jobs;
using OfferPrice.Auction.Api.Settings;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Auction.Infrastructure;
using OfferPrice.Auction.Infrastructure.Events;
using OfferPrice.Auction.Infrastructure.Repositories;
using OfferPrice.Events.RabbitMq;

namespace OfferPrice.Auction.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services, AuctionSettings settings)
    {
        services.AddSingleton(settings);
        services.AddHostedService<StartAuctionJob>();
        services.AddHostedService<FinishAuctionJob>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, DatabaseSettings settings)
    {
        services.AddSingleton(settings);

        ConventionRegistry.Register("IgnoreExtraElementsConvention", new ConventionPack
        {
            new IgnoreExtraElementsConvention(true)
        }, _ => true);

        services.AddSingleton(_ => new MongoClient(settings.ConnectionString).GetDatabase(settings.Name));

        services.AddSingleton<ILotRepository, LotRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<ICommentRepository, CommentRepository>();

        return services;
    }

    public static IServiceCollection RegisterRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.Get<AppSettings>();

        services.AddRabbitMqProducer(settings.RabbitMq);

        services.AddRabbitMqConsumer<ProductCreatedEventConsumer>(settings.RabbitMq);
        services.AddRabbitMqConsumer<UserCreatedEventConsumer>(settings.RabbitMq);

        return services;
    }
}