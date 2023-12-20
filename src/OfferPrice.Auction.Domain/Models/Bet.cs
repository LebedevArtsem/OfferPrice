using System;

namespace OfferPrice.Auction.Domain.Models;

public class Bet
{
    public Bet()
    {
    }

    public Bet(User user, decimal raise)
    {
        User = user;
        Raise = raise;
        Timestamp = DateTime.UtcNow;
    }

    public User User { get; set; }

    public decimal Raise { get; set; }

    public DateTime Timestamp { get; set; }
}

