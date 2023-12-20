using System;

namespace OfferPrice.Auction.Api.Models;

public class Bet
{
    public Bet(Domain.Models.Bet bet)
    {
        User = User.FromDomain(bet.User);
        Raise = bet.Raise;
        Timestamp = bet.Timestamp;
    }

    public User User { get; set; }
    public decimal Raise { get; set; }
    public DateTime Timestamp { get; set; }
}