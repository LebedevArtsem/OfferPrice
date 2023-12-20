using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq.Options;
using System;

namespace OfferPrice.Events.RabbitMq.Helpers;

public class RabbitMqEventResolver : IEventResolver
{
    private readonly RabbitMqSettings _options;

    public RabbitMqEventResolver(RabbitMqSettings options)
    {
        _options = options;
    }

    public EventOptions Resolve<T>() where T : Event
    {
        if (_options.Events.TryGetValue(typeof(T).Name, out var eventOptions))
        {
            return eventOptions;
        }

        throw new ArgumentOutOfRangeException(nameof(eventOptions), "There are not available queues");
    }
}

