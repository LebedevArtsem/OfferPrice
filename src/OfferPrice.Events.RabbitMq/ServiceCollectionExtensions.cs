using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq.Helpers;
using OfferPrice.Events.RabbitMq.Options;
using System.Linq;

namespace OfferPrice.Events.RabbitMq;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqProducer(this IServiceCollection services, RabbitMqSettings settings)
    {
        services.TryAddSingleton(settings);
        services.TryAddSingleton<IEventResolver, RabbitMqEventResolver>();
        services.AddSingleton<IProducer, RabbitMqProducer>();

        return services;
    }

    public static IServiceCollection AddRabbitMqConsumer<TConsumer>(this IServiceCollection services,
        RabbitMqSettings settings)
        where TConsumer : class, IConsumer
    {
        services.TryAddSingleton(settings);
        services.TryAddSingleton<IEventResolver, RabbitMqEventResolver>();
        services.AddSingleton<IConsumer, TConsumer>();
        
        services.TryAddConsumerService();

        return services;
    }

    private static void TryAddConsumerService(this IServiceCollection services)
    {
        var isRegistered = services.FirstOrDefault(s => s.ImplementationType == typeof(ConsumerService));

        if (isRegistered == null)
        {
            services.AddHostedService<ConsumerService>();
        }
    }
}