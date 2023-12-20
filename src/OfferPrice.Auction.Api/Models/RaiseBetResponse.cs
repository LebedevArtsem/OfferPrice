using System;
using System.Linq;

namespace OfferPrice.Auction.Api.Models;

public class RaiseBetResponse
{
    public RaiseBetResponse(Domain.Models.Lot lot, DateTime timeToFinish)
    {
        var lastBet = lot.BetHistory.Last();
        
        BetOwner = User.FromDomain(lastBet.User);
        BetTimestamp = lastBet.Timestamp;
        ProductPrice = lot.Price;
        TimeToFinish = timeToFinish;
    }
    public User BetOwner { get; set; }
    public DateTime BetTimestamp { get; set; }
    public decimal ProductPrice { get; set; }
    public DateTime TimeToFinish { get; set; }
}