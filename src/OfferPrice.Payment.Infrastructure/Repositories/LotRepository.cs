using MongoDB.Driver;
using OfferPrice.Payment.Domain.Interfaces;
using OfferPrice.Payment.Domain.Models;

namespace OfferPrice.Payment.Infrastructure.Repositories;

public class LotRepository : ILotRepository
{
    private readonly IMongoCollection<Lot> _lotRepository;

    public LotRepository(IMongoDatabase database)
    {
        _lotRepository = database.GetCollection<Lot>("lots");
    }

    public Task Create(Lot lot, CancellationToken token)
    {
        return _lotRepository.InsertOneAsync(lot, token);
    }

    public Task<Lot> GetById(string id, CancellationToken token)
    {
        return _lotRepository
            .Find(x => x.Id == id)
            .SingleOrDefaultAsync(token);
    }
}

