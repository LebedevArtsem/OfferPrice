using System;

namespace OfferPrice.Auction.Domain.Models;

public class SubComment
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string LotId { get; set; }

    public string Text { get; set; }

    public DateTime CreationDate { get; set; }
}

