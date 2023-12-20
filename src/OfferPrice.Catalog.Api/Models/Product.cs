namespace OfferPrice.Catalog.Api.Models;

public class Product
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public string User { get; set; }

    public string Category { get; set; }

    public string Brand { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; }
}