using OfferPrice.Common;
using System.Collections.Generic;
using System.Linq;

namespace OfferPrice.Auction.Api.Models.Comment;

public class CommentResponse
{
    public CommentResponse(PageResult<Domain.Models.Comment> pageResult)
    {
        Comments = pageResult?.Items.Select(x => new Comment(x)).ToList();
        Total = pageResult.Total;
    }

    public List<Comment> Comments { get; set; }

    public long Total { get; set; }
}

