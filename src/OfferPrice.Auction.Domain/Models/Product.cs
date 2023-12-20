namespace OfferPrice.Auction.Domain.Models;
public class Product
{
    public Product()
    {
    }

    public Product(Events.Models.Product product, User user)
    {
        Id = product.Id;
        Name = product.Name;
        Image = product.Image;
        User = user;
        Brand = product.Brand;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public User User { get; set; }
    public string Brand { get; set; }
}

