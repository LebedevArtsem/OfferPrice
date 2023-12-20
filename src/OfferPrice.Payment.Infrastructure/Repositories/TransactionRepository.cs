using MongoDB.Driver;
using OfferPrice.Common;
using OfferPrice.Payment.Domain.Interfaces;
using OfferPrice.Payment.Domain.Models;
using OfferPrice.Payment.Domain.Queries;

namespace OfferPrice.Payment.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IMongoCollection<Transaction> _transaction;

    public TransactionRepository(IMongoDatabase database)
    {
        _transaction = database.GetCollection<Transaction>("transactions");
    }

    public Task Create(Transaction transaction, CancellationToken token)
    {
        return _transaction.InsertOneAsync(transaction, token);
    }

    public async Task<PageResult<Transaction>> Find(FindTransacitonQuery query, CancellationToken token)
    {
        var userFilter = Builders<Transaction>.Filter.Eq(x => x.UserId, query.UserId);

        var statusFilter = string.IsNullOrEmpty(query.Status) ?
            Builders<Transaction>.Filter.Empty :
            Builders<Transaction>.Filter.Eq(x => x.Status, query.Status);

        var dateStartFilter = Builders<Transaction>.Filter.Gte(x => x.Date, query.StartDate);
        var dateEndFilter = Builders<Transaction>.Filter.Lte(x => x.Date, query.EndDate);

        var filter = Builders<Transaction>.Filter.And(userFilter, statusFilter, dateStartFilter, dateEndFilter);

        var totalTask = _transaction.CountDocumentsAsync(filter);
        var lotsTask = _transaction.Find(filter)
           .Skip((query.Paging.Page - 1) * query.Paging.PerPage)
           .Limit(query.Paging.PerPage)
           .ToListAsync(token);

        await Task.WhenAll(totalTask, lotsTask);

        return new PageResult<Transaction>(
            page: query.Paging.Page,
            perPage: query.Paging.PerPage,
            total: totalTask.Result,
            items: lotsTask.Result
        );
    }

    public Task<Transaction> GetById(string id, CancellationToken token)
    {
        return _transaction.Find(Builders<Transaction>.Filter.Eq(x => x.Id, id))
                     .SingleOrDefaultAsync(token);
    }
}

