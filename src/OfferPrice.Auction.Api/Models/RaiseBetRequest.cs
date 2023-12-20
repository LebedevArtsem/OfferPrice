namespace OfferPrice.Auction.Api.Models;

public class RaiseBetRequest
{
    public string UserId { get; set; }
    public decimal Raise { get; set; }
}