namespace OfferPrice.Catalog.Domain;
public class Product
{
    public Product()
    {
        Id = Guid.NewGuid().ToString();
        Status = ProductStatus.Created;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public string User { get; set; }

    public string Category { get; set; }

    public string Brand { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; }

    public Events.Models.Product ToEvent()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Brand = Brand,
            Image = Image,
            Price = Price,
            User = User
        };
    }
}

