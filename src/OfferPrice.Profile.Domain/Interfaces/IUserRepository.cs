using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Domain.Interfaces;

public interface IUserRepository
{

    Task<User> Get(string id, CancellationToken token);

    Task<User> GetByEmail(string email, CancellationToken token);

    Task<User> GetByEmailAndPassword(string email, string password, CancellationToken token);

    Task Create(User user, CancellationToken token);

    Task Update(User user, CancellationToken token);
}

