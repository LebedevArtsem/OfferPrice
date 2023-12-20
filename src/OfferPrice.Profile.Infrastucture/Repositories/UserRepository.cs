
using MongoDB.Driver;
using OfferPrice.Profile.Domain.Interfaces;
using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("users");
    }

    public Task<User> Get(string id, CancellationToken token)
    {
        return _users.Find(Builders<User>.Filter.Eq(x => x.Id, id))
                     .SingleOrDefaultAsync(token);
    }

    public Task<User> GetByEmail(string email, CancellationToken token)
    {
        return _users.Find(Builders<User>.Filter.Where(x => x.Email == email))
                     .SingleOrDefaultAsync();
    }

    public Task<User> GetByEmailAndPassword(string email, string password, CancellationToken token)
    {
        return _users.Find(Builders<User>.Filter.Where(x => x.Email == email && x.PasswordHash == password))
                     .SingleOrDefaultAsync();
    }

    public async Task Create(User user, CancellationToken token)
    {
        await _users.InsertOneAsync(user, cancellationToken: token);
    }

    public async Task Update(User user, CancellationToken token)
    {
        await _users.UpdateOneAsync(
            Builders<User>.Filter.Where(x => x.Id == user.Id),
            Builders<User>.Update.Set(x => x.Name, user.Name)
                                 .Set(x => x.LastName, user.LastName)
                                 .Set(x => x.MiddleName, user.MiddleName)
                                 .Set(x => x.Email, user.Email)
                                 .Set(x => x.Phone, user.Phone),
            cancellationToken: token);
    }
}

