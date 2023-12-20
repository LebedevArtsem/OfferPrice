using OfferPrice.Common;
using OfferPrice.Payment.Domain.Models;
using OfferPrice.Payment.Domain.Queries;

namespace OfferPrice.Payment.Domain.Interfaces;

public interface ITransactionRepository
{
    Task Create(Transaction transaction, CancellationToken token);

    Task<Transaction> GetById(string id, CancellationToken token);

    Task<PageResult<Transaction>> Find(FindTransacitonQuery query, CancellationToken token);
}

