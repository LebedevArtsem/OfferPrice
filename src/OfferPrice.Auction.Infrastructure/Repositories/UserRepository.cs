using MongoDB.Driver;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Auction.Domain.Models;

namespace OfferPrice.Auction.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("users");
    }

    public Task<User> Get(string id, CancellationToken token)
    {
        return _users.Find(Builders<User>.Filter.Eq(x => x.Id, id)).SingleOrDefaultAsync(token);
    }

    public Task Create(User user, CancellationToken token)
    {
        var utc = DateTime.UtcNow;
        user.Version = 1;
        user.Created = utc;
        user.Updated = utc;

        return _users.InsertOneAsync(user, cancellationToken: token);
    }

    public Task Update(User user, CancellationToken token)
    {
        user.Updated = DateTime.UtcNow;

        return _users.UpdateOneAsync(
            Builders<User>.Filter.Where(x => x.Id == user.Id),
            Builders<User>.Update.Set(x => x.Email, user.Email),
            cancellationToken: token);
    }
}