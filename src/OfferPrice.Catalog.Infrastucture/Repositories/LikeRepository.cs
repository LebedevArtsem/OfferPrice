using MongoDB.Driver;
using OfferPrice.Catalog.Domain;

namespace OfferPrice.Catalog.Infrastructure.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly IMongoCollection<Like> _likes;

    public LikeRepository(IMongoDatabase database)
    {
        _likes = database.GetCollection<Like>("likes");
        _likes.Indexes.CreateOne(new CreateIndexModel<Like>(
            Builders<Like>.IndexKeys.Ascending(l => l.ProductId).Ascending(l => l.UserId)
        ));
    }

    public Task Create(Like like, CancellationToken token)
    {
        return _likes.InsertOneAsync(
            like,
            cancellationToken: token
        );
    }

    public Task Delete(string productId, string userId, CancellationToken token)
    {
        var filter = Builders<Like>.Filter.Eq(x => x.UserId, userId)
                     & Builders<Like>.Filter.Eq(x => x.ProductId, productId);
        return _likes.DeleteOneAsync(filter, cancellationToken: token);
    }

    public Task<Like> Get(string productId, string userId, CancellationToken token)
    {
        return _likes.Find(
            Builders<Like>.Filter.Where(x => x.UserId == userId && x.ProductId == productId)
        ).FirstOrDefaultAsync(token);
    }

    public Task<long> GetCount(string productId, CancellationToken token)
    {
        return _likes.CountDocumentsAsync(
            Builders<Like>.Filter.Where(x => x.ProductId == productId),
            cancellationToken: token
        );
    }
}