using OfferPrice.Auction.Infrastructure;
using OfferPrice.Events.RabbitMq.Options;

namespace OfferPrice.Auction.Api.Settings;

public class AppSettings
{
    public DatabaseSettings Database { get; set; }
    public RabbitMqSettings RabbitMq { get; set; }
    public AuctionSettings Auction { get; set; }
}