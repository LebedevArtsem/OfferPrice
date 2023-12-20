namespace OfferPrice.Payment.Domain.Models;

public class Lot
{
    public string Id { get; set; }

    public string WinnerId { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; }
}

