using System.Collections.Generic;

namespace OfferPrice.Events.RabbitMq.Options;

public class RabbitMqSettings
{
    public string Host { get; set; }

    public IReadOnlyDictionary<string, string> Exchanges { get; set; }

    public IReadOnlyDictionary<string, EventOptions> Events { get; set; }
}