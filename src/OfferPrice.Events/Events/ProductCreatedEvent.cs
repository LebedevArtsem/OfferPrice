using OfferPrice.Events.Models;

namespace OfferPrice.Events.Events;

public class ProductCreatedEvent : Event
{
    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; set; }
}