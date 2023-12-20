using OfferPrice.Auction.Domain.Queries;

namespace OfferPrice.Auction.Api.Models.Comment;

public class FindCommentRequest
{
    public FindCommentsQuery ToQuery()
    {
        return new()
        {
            Paging = new(Page, PerPage)
        };
    }
    public int Page { get; set; }

    public int PerPage { get; set; }
}

