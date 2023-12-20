using System;

namespace OfferPrice.Auction.Api.Models;
public class AuctionStartedResponse
{
    public AuctionStartedResponse(decimal price, DateTime timeToFinish)
    {
        ProductPrice = price;
        TimeToFinish = timeToFinish;
    }
    
    public decimal ProductPrice { get; set; }
    public DateTime TimeToFinish { get; set; }
}

