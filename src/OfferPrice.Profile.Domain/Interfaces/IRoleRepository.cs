using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Domain.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> Get(CancellationToken token);

    Task<Role> GetByName(string roleName, CancellationToken token);
}


