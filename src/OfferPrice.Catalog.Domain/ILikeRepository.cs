namespace OfferPrice.Catalog.Domain;
public interface ILikeRepository
{
    Task<Like> Get(string productId, string userId, CancellationToken token);

    Task<long> GetCount(string productId, CancellationToken token);

    Task Create(Like like, CancellationToken token);

    Task Delete(string productId, string userId, CancellationToken token);
}

