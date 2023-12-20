using OfferPrice.Events.RabbitMq.Options;

namespace OfferPrice.Profile.Api.Settings
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }
        public RabbitMqSettings RabbitMq { get; set; }
    }
}
