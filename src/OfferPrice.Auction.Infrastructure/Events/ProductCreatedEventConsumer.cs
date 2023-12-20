using Microsoft.Extensions.Logging;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Auction.Domain.Models;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq;

namespace OfferPrice.Auction.Infrastructure.Events;

public class ProductCreatedEventConsumer : RabbitMqConsumer<ProductCreatedEvent>
{
    private readonly ILotRepository _lotRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ProductCreatedEventConsumer> _logger;

    public ProductCreatedEventConsumer(
        ILotRepository lotRepository,
        IUserRepository userRepository,
        IEventResolver eventResolver,
        ILogger<ProductCreatedEventConsumer> logger
    ) : base(eventResolver, logger)
    {
        _lotRepository = lotRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    protected override async Task Execute(ProductCreatedEvent message, CancellationToken cancellationToken)
    {
        var lot = await _lotRepository.GetByProductId(message.Product.Id, cancellationToken);
        if (lot != null)
        {
            _logger.LogInformation("Lot {lot_id} is already exists for product {product_id}", lot.Id, lot.Product.Id);
            return;
        }

        var user = await _userRepository.Get(message.Product.User, cancellationToken);
        if (user == null)
        {
            _logger.LogError("User {user_id} not found", message.Product.User);
            throw new Exception($"User {message.Product.User} not found");
        }

        lot = new Lot
        {
            Product = new Product(message.Product, user),
            Price = message.Product.Price
        };
        await _lotRepository.Create(lot, cancellationToken);

        _logger.LogInformation("Lot {lot} was successfully created", lot);
    }
}