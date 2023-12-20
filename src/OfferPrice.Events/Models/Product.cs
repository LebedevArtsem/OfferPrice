namespace OfferPrice.Events.Models;

public class Product
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public string User { get; set; }

    public string Brand { get; set; }

    public decimal Price { get; set; }
}