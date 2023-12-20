using OfferPrice.Common;

namespace OfferPrice.Catalog.Domain;
public interface IProductRepository
{
    Task<PageResult<Product>> Get(string name, string username, string category, int page, int perPage, CancellationToken token);

    Task<Product> GetById(string id, CancellationToken token);

    Task Insert(Product product, CancellationToken token);

    Task Update(Product product, CancellationToken token);
}

