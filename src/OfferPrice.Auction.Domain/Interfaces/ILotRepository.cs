using OfferPrice.Auction.Domain.Models;
using OfferPrice.Auction.Domain.Queries;
using OfferPrice.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Domain.Interfaces;
public interface ILotRepository
{
    Task<PageResult<Lot>> Find(FindLotsQuery query, CancellationToken token);
    Task<Lot> Get(string id, CancellationToken token);
    Task<Lot> GetByProductId(string productId, CancellationToken token);
    Task Create(Lot lot, CancellationToken token);
    Task Update(Lot lot, CancellationToken token);
    Task Delete(string id, CancellationToken token);
    Task<List<Lot>> GetNonStarted(DateTime until, CancellationToken cancellationToken);
    Task<List<Lot>> GetNonFinished(DateTime until, CancellationToken cancellationToken);
}

