using MongoDB.Driver;
using OfferPrice.Catalog.Domain;
using OfferPrice.Common;

namespace OfferPrice.Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(IMongoDatabase database)
    {
        _products = database.GetCollection<Product>("products");
    }

    public async Task<PageResult<Product>> Get
        (string name, string username, string category, int page, int perPage, CancellationToken token)
    {
        var filterByName = !string.IsNullOrWhiteSpace(name)
            ? Builders<Product>.Filter.Eq(x => x.Name, name)
            : Builders<Product>.Filter.Empty;
        var filterByUsername = !string.IsNullOrWhiteSpace(username)
            ? Builders<Product>.Filter.Eq(x => x.User, username)
            : Builders<Product>.Filter.Empty;
        var filterByCategory = !string.IsNullOrWhiteSpace(category)
            ? Builders<Product>.Filter.Eq(x => x.Category, category)
            : Builders<Product>.Filter.Empty;
        var filterByStatus = Builders<Product>.Filter.Ne(x => x.Status, ProductStatus.PlayedOut);

        var filter = Builders<Product>.Filter.And(filterByName, filterByUsername, filterByCategory, filterByStatus);

        var totalTask = _products.CountDocumentsAsync(filter, cancellationToken: token);
        var productsTask = _products.Find(filter)
            .Skip((page - 1) * perPage)
            .Limit(perPage)
            .ToListAsync(token);

        await Task.WhenAll(totalTask, productsTask);

        return new PageResult<Product>(page, perPage, totalTask.Result, productsTask.Result);
    }

    public Task<Product> GetById(string id, CancellationToken token)
    {
        return _products.Find(x => x.Id == id).FirstOrDefaultAsync(token);
    }

    public Task Insert(Product product, CancellationToken token)
    {
        return _products.InsertOneAsync(product, cancellationToken: token);
    }

    public Task Update(Product product, CancellationToken token)
    {
        return _products.UpdateOneAsync(
            Builders<Product>.Filter.Where(x => x.Id == product.Id),
            Builders<Product>.Update.Set(x => x.Name, product.Name)
                .Set(x => x.Category, product.Category)
                .Set(x => x.Description, product.Description)
                .Set(x => x.Image, product.Image)
                .Set(x => x.Brand, product.Brand)
                .Set(x => x.Price, product.Price)
                .Set(x => x.Status, product.Status),
            cancellationToken: token
        );
    }
}