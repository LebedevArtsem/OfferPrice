
using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Domain.Interfaces;

public interface INotificationRepository
{
    Task<Notification> GetNotifications(string userId, CancellationToken token);

    Task CreateNotifications(string userId, CancellationToken token);

    Task SwitchReminders(string userId, CancellationToken token);

    Task SwitchPurchaces(string userId, CancellationToken token);
}
