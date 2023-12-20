using MongoDB.Driver;
using OfferPrice.Profile.Domain.Interfaces;
using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IMongoCollection<Role> _roles;

    public RoleRepository(IMongoDatabase database)
    {
        _roles = database.GetCollection<Role>("roles");
    }

    public Task<List<Role>> Get(CancellationToken token)
    {
        return _roles.AsQueryable().ToListAsync(token);
    }

    public Task<Role> GetByName(string roleName, CancellationToken token)
    {
        return _roles.Find(Builders<Role>.Filter.Where(x => x.Name == roleName))
                     .SingleOrDefaultAsync(token);
    }
}

