using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Domain.Interfaces;

public interface IDoubleAuthRepository
{
    Task Create(DoubleAuth doubleAuth, CancellationToken cancellationToken);

    Task<DoubleAuth> GetByUserEmail(string userEmail,  CancellationToken cancellationToken);

    Task Update(DoubleAuth doubleAuth, CancellationToken cancellationToken);
}

