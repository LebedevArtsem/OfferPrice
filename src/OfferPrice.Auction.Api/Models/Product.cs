namespace OfferPrice.Auction.Api.Models;

public class Product
{
    public Product(Domain.Models.Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Image = product.Image;
        User = User.FromDomain(product.User);
        Brand = product.Brand;
    }
    
    public string Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public User User { get; set; }
    public string Brand { get; set; }
}