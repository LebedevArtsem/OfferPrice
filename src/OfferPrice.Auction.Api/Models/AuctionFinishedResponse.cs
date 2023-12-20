namespace OfferPrice.Auction.Api.Models;

public class AuctionFinishedResponse
{
    public AuctionFinishedResponse(Domain.Models.Lot lot)
    {
        Status = lot.Status;
        Winner = User.FromDomain(lot.Winner);
        ProductPrice = lot.Price;
    }
    
    public string Status { get; set; }
    public User Winner { get; set; }
    public decimal ProductPrice { get; set; }
}