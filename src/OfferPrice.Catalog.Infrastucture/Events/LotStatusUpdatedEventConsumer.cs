using Microsoft.Extensions.Logging;
using OfferPrice.Catalog.Domain;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq;
using OfferPrice.Events.RabbitMq.Options;

namespace OfferPrice.Catalog.Infrastructure.Events;

public class LotStatusUpdatedEventConsumer : RabbitMqConsumer<LotStatusUpdatedEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<LotStatusUpdatedEventConsumer> _logger;

    public LotStatusUpdatedEventConsumer(
        IProductRepository productRepository,
        IEventResolver eventResolver,
        RabbitMqSettings settings,
        ILogger<LotStatusUpdatedEventConsumer> logger
    ) : base(eventResolver, logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    protected override async Task Execute(LotStatusUpdatedEvent message, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetById(message.ProductId, cancellationToken);
        if (product == null)
        {
            _logger.LogInformation("Product {lot_id} not found", message.ProductId);
            return;
        }

        product.Status = ProductStatus.GetFromLotStatus(message.Status);
        if (string.IsNullOrWhiteSpace(product.Status))
        {
            return;
        }
        await _productRepository.Update(product, cancellationToken);

        _logger.LogInformation("Product {product_id} was successfully updated with status {product_status}", product.Id, product.Status);
    }
}