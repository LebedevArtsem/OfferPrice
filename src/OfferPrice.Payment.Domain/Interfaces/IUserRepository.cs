using OfferPrice.Payment.Domain.Models;

namespace OfferPrice.Payment.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetById(string id, CancellationToken token);

    Task Create(User user, CancellationToken token);

    Task Update(User user, CancellationToken token);
}

