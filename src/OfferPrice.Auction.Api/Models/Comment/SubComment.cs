using System;

namespace OfferPrice.Auction.Api.Models.Comment;

public class SubComment
{
    public SubComment(Domain.Models.SubComment subComment)
    {
        Id = subComment.Id;
        UserId = subComment.UserId;
        LotId = subComment.LotId;
        Text = subComment.Text;
        CreationDate = subComment.CreationDate;
    }

    public int Id { get; set; }

    public string UserId { get; set; }

    public string LotId { get; set; }

    public string Text { get; set; }

    public DateTime CreationDate { get; set; }
}

