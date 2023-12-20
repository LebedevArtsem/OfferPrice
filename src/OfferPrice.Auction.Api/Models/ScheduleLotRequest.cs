using System;

namespace OfferPrice.Auction.Api.Models;

public class ScheduleLotRequest
{
    public DateTime Start { get; set; }
    public string UserId { get; set; }
}