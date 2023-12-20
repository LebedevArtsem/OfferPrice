using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OfferPrice.Events.RabbitMq;

public class RabbitMqProducer : IProducer
{
    private readonly IEventResolver _eventResolver;
    private readonly RabbitMqSettings _settings;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RabbitMqProducer(IEventResolver eventResolver, RabbitMqSettings settings)
    {
        _eventResolver = eventResolver;
        _settings = settings;
    }

    public void SendMessage<T>(T message) where T : Event
    {
        var factory = new ConnectionFactory { HostName = _settings.Host };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var eventSettings = _eventResolver.Resolve<T>();

        var exchange = eventSettings.Exchange;
        var key = eventSettings.Key;

        channel.ExchangeDeclare(exchange, ExchangeType.Direct);

        var json = JsonSerializer.Serialize(message, _jsonSerializerOptions);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: exchange,
            routingKey: key,
            basicProperties: null,
            body: body
        );
    }
}