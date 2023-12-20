using OfferPrice.Payment.Domain.Models;

namespace OfferPrice.Payment.Domain.Interfaces;

public interface ILotRepository
{
    Task<Lot> GetById(string id, CancellationToken token);

    Task Create(Lot lot, CancellationToken token);
}

