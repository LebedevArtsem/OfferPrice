using OfferPrice.Auction.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> Get(string id, CancellationToken token);

    Task Update(User user, CancellationToken token);

    Task Create(User user, CancellationToken token);
}