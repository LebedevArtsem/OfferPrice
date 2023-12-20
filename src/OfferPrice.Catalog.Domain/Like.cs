namespace OfferPrice.Catalog.Domain;
public class Like
{
    public Like()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    public string UserId { get; set; }

    public string ProductId { get; set; }
}

