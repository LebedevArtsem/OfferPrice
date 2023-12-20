using System;
using System.Collections.Generic;

namespace OfferPrice.Auction.Domain.Models;

public class Comment
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string LotId { get; set; }

    public string Text { get; set; }

    public DateTime CreationDate { get; set; }

    public List<SubComment> SubComments { get; set; }
}
