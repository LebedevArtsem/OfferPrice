using MongoDB.Driver;
using OfferPrice.Auction.Domain;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Auction.Domain.Models;
using OfferPrice.Auction.Domain.Queries;
using OfferPrice.Common;

namespace OfferPrice.Auction.Infrastructure.Repositories;

public class LotRepository : ILotRepository
{
    private readonly IMongoCollection<Lot> _lots;

    public LotRepository(IMongoDatabase database)
    {
        _lots = database.GetCollection<Lot>("lots");
        _lots.Indexes.CreateMany(new[]
        {
            new CreateIndexModel<Lot>(
                Builders<Lot>.IndexKeys.Ascending(l => l.Product.Id),
                new CreateIndexOptions { Unique = true }
            ),
            new CreateIndexModel<Lot>(
                Builders<Lot>.IndexKeys.Ascending(l => l.Start).Ascending(l => l.Status)
            ),
            new CreateIndexModel<Lot>(
                Builders<Lot>.IndexKeys.Ascending(l => l.Updated).Ascending(l => l.Status)
            )
        });
    }

    public Task Create(Lot lot, CancellationToken token)
    {
        var utc = DateTime.UtcNow;
        lot.Version = 1;
        lot.Created = utc;
        lot.Updated = utc;

        return _lots.InsertOneAsync(lot, cancellationToken: token);
    }

    public Task Delete(string id, CancellationToken token)
    {
        return _lots.DeleteOneAsync(Builders<Lot>.Filter.Eq(x => x.Id, id), cancellationToken: token);
    }

    public Task<Lot> Get(string id, CancellationToken token)
    {
        return _lots.Find(Builders<Lot>.Filter.Eq(x => x.Id, id)).SingleOrDefaultAsync(token);
    }

    public Task<Lot> GetByProductId(string productId, CancellationToken token)
    {
        return _lots.Find(Builders<Lot>.Filter.Eq(x => x.Product.Id, productId)).FirstOrDefaultAsync(token);
    }

    public async Task<PageResult<Lot>> Find(FindLotsQuery query, CancellationToken token)
    {
        var ownerFilter = string.IsNullOrWhiteSpace(query.ProductOwnerId)
            ? Builders<Lot>.Filter.Empty
            : Builders<Lot>.Filter.Eq(l => l.Product.User.Id, query.ProductOwnerId);
        var winnerFilter = string.IsNullOrWhiteSpace(query.WinnerId)
            ? Builders<Lot>.Filter.Empty
            : Builders<Lot>.Filter.Eq(l => l.Winner.Id, query.WinnerId);
        var statusesFilter = query.Statuses.Length == 0
            ? Builders<Lot>.Filter.Empty
            : Builders<Lot>.Filter.In(l => l.Status, query.Statuses);

        var filter = Builders<Lot>.Filter.And(ownerFilter, statusesFilter, winnerFilter);
        var sort = Builders<Lot>.Sort.Ascending(l => l.Updated);

        var totalTask = _lots.CountDocumentsAsync(filter, cancellationToken: token);
        var lotsTask = _lots.Find(filter)
            .Sort(sort)
            .Skip((query.Paging.Page - 1) * query.Paging.PerPage)
            .Limit(query.Paging.PerPage)
            .ToListAsync(token);

        await Task.WhenAll(totalTask, lotsTask);
        return new PageResult<Lot>(
            page: query.Paging.Page,
            perPage: query.Paging.PerPage,
            total: totalTask.Result,
            items: lotsTask.Result
        );
    }

    public Task<List<Lot>> GetNonStarted(DateTime until, CancellationToken cancellationToken)
    {
        var untilFilter = Builders<Lot>.Filter.Lte(l => l.Start, until);
        var statusFilter = Builders<Lot>.Filter.Eq(l => l.Status, LotStatus.Planned);

        return _lots.Find(untilFilter & statusFilter).ToListAsync(cancellationToken);
    }

    public Task<List<Lot>> GetNonFinished(DateTime until, CancellationToken cancellationToken)
    {
        var fromFilter = Builders<Lot>.Filter.Lt(l => l.Updated, until);
        var statusFilter = Builders<Lot>.Filter.Eq(l => l.Status, LotStatus.Started);

        return _lots.Find(fromFilter & statusFilter).ToListAsync(cancellationToken);
    }

    public async Task Update(Lot lot, CancellationToken token)
    {
        var version = lot.Version;

        lot.Version++;
        lot.Updated = DateTime.UtcNow;

        await _lots.UpdateOneAsync(
            Builders<Lot>.Filter.Eq(x => x.Id, lot.Id) & Builders<Lot>.Filter.Eq(x => x.Version, version),
            Builders<Lot>.Update
                .Set(x => x.Product, lot.Product)
                .Set(x => x.Winner, lot.Winner)
                .Set(x => x.Price, lot.Price)
                .Set(x => x.BetHistory, lot.BetHistory)
                .Set(x => x.Start, lot.Start)
                .Set(x => x.End, lot.End)
                .Set(x => x.Status, lot.Status)
                .Set(x => x.Version, lot.Version)
                .Set(x => x.Updated, lot.Updated),
            cancellationToken: token
        );
    }
}