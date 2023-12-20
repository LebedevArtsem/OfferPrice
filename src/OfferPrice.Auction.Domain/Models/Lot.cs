using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferPrice.Auction.Domain.Models;
public class Lot
{
    public Lot()
    {
        Id = Guid.NewGuid().ToString();
        BetHistory = new List<Bet>();
        Status = LotStatus.Created;
    }

    public string Id { get; set; }

    public Product Product { get; set; }

    public User Winner { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; }

    public List<Bet> BetHistory { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public int Version { get; set; }

    public void Schedule(DateTime date)
    {
        Start = date;
        Status = LotStatus.Planned;
    }

    public Bet RaiseBet(User user, decimal raise)
    {
        var bet = new Bet(user, raise);

        Price += raise;
        BetHistory.Add(bet);

        return bet;
    }

    public void Begin()
    {
        Status = LotStatus.Started;
    }

    public bool IsStarted()
    {
        return Status == LotStatus.Started;
    }

    public void Finish()
    {
        var winner = BetHistory.LastOrDefault()?.User;
        if (winner == null)
        {
            Status = LotStatus.Unsold;
            return;
        }

        Status = LotStatus.Sold;
        Winner = winner;
        End = DateTime.UtcNow;
    }

    public bool IsFinished()
    {
        return Status is LotStatus.Sold or LotStatus.Unsold;
    }

    public bool IsSold()
    {
        return Status is LotStatus.Sold;
    }

    public void Deliver()
    {
        Status = LotStatus.Delivered;
    }
}
