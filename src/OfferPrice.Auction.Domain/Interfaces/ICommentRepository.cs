using OfferPrice.Auction.Domain.Models;
using OfferPrice.Auction.Domain.Queries;
using OfferPrice.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Domain.Interfaces;

public interface ICommentRepository
{
    Task<PageResult<Comment>> GetAuctionComments(string lotId, FindCommentsQuery query, CancellationToken cancellationToken);

    Task AddComment(Comment comment, CancellationToken cancellationToken);

    Task AddSubComment(string commentId, SubComment subComment, CancellationToken cancellationToken);
}

