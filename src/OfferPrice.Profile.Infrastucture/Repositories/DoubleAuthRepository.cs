using MongoDB.Driver;
using OfferPrice.Profile.Domain.Interfaces;
using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Infrastructure.Repositories;

public class DoubleAuthRepository : IDoubleAuthRepository
{
    private readonly IMongoCollection<DoubleAuth> _doubleAuths;

    public DoubleAuthRepository(IMongoDatabase database)
    {
        _doubleAuths = database.GetCollection<DoubleAuth>("double_auth_codes");
    }

    public Task Create(DoubleAuth doubleAuth, CancellationToken cancellationToken)
    {
        return _doubleAuths.InsertOneAsync(doubleAuth, cancellationToken: cancellationToken);
    }

    public Task<DoubleAuth> GetByUserEmail(string userEmail, CancellationToken cancellationToken)
    {
        return _doubleAuths
            .Find(x => x.UserEmail == userEmail &&
                  x.CreationDate.AddMinutes(5) > DateTime.Now &&
                  x.CreationDate < DateTime.Now)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task Update(DoubleAuth doubleAuth, CancellationToken cancellationToken)
    {
        var timeFilter = Builders<DoubleAuth>.Filter.Gte(x => x.CreationDate, DateTime.UtcNow);
        var emailFilter = Builders<DoubleAuth>.Filter.Eq(x => x.UserEmail, doubleAuth.UserEmail);

        var filter = Builders<DoubleAuth>.Filter.And(timeFilter, emailFilter); 

        return _doubleAuths.UpdateOneAsync(
            filter,
            Builders<DoubleAuth>.Update
            .Set(x => x.Code, doubleAuth.Code)
            .Set(x => x.CreationDate, doubleAuth.CreationDate),
             cancellationToken: cancellationToken
            );
    }
}

