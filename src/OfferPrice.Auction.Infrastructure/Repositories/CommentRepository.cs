using MongoDB.Driver;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Auction.Domain.Models;
using OfferPrice.Auction.Domain.Queries;
using OfferPrice.Common;
using OfferPrice.Common.Exceptions;

namespace OfferPrice.Auction.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly IMongoCollection<Comment> _comments;

    private readonly IMongoCollection<Lot> _lots;

    public CommentRepository(IMongoDatabase database)
    {
        _comments = database.GetCollection<Comment>("comments");
        _lots = database.GetCollection<Lot>("lots");
    }

    public async Task AddComment(Comment comment, CancellationToken cancellationToken)
    {
        await CheckIfLotExist(comment.LotId, cancellationToken); // Is it necessary?

        await _comments.InsertOneAsync(comment, cancellationToken: cancellationToken);
    }

    public async Task AddSubComment(string commentId, SubComment subComment, CancellationToken cancellationToken)
    {
        await CheckIfLotExist(subComment.LotId, cancellationToken); // Is it necessary?

        var comment = await _comments
            .Find(x => x.LotId == subComment.LotId && x.Id == commentId)
            .SingleOrDefaultAsync(cancellationToken);

        if (comment == null)
        {
            throw new EntityNotFoundException("Comment is not found");
        }

        comment.SubComments.Add(subComment);

        await _comments.UpdateOneAsync(
            Builders<Comment>.Filter.Eq(x => x.Id, comment.Id),
            Builders<Comment>.Update.Set(x => x.SubComments, comment.SubComments),
            cancellationToken: cancellationToken);
    }

    public async Task<PageResult<Comment>> GetAuctionComments(string lotId, FindCommentsQuery query, CancellationToken cancellationToken)
    {
        var lotFilter = Builders<Comment>.Filter.Eq(l => l.LotId, lotId);

        var filter = Builders<Comment>.Filter.And(lotFilter);

        var totalTask = _comments.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var sort = Builders<Comment>.Sort.Ascending(l => l.CreationDate);

        var commentsTask = _comments.Find(filter)
            .Sort(sort)
            .Skip((query.Paging.Page - 1) * query.Paging.PerPage)
            .Limit(query.Paging.PerPage)
            .ToListAsync(cancellationToken);

        await Task.WhenAll(totalTask, commentsTask);
        return new PageResult<Comment>(
            page: query.Paging.Page,
            perPage: query.Paging.PerPage,
            total: totalTask.Result,
            items: commentsTask.Result
        );
    }

    private async Task CheckIfLotExist(string lotId, CancellationToken token)
    {
        var lot = await _lots.Find(x => x.Id == lotId).SingleOrDefaultAsync();

        if (lot is null)
        {
            throw new EntityNotFoundException("Lot is not found");
        }
    }
}

