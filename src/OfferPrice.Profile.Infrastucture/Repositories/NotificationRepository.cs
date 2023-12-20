using MongoDB.Driver;
using OfferPrice.Profile.Domain.Interfaces;
using OfferPrice.Profile.Domain.Models;

namespace OfferPrice.Profile.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<Notification> _notifications;

    public NotificationRepository(IMongoDatabase database)
    {
        _notifications = database.GetCollection<Notification>("notifications");
    }

    public async Task CreateNotifications(string userId, CancellationToken token)
    {
        var notification = await FindNotification(userId, token);

        if (notification != null)
        {
            // log
            return;
        }

        notification = new Notification();

        notification.Id = Guid.NewGuid().ToString();
        notification.UserId = userId;
        notification.Purchaces = true;
        notification.Reminders = true;

        await _notifications.InsertOneAsync(notification, token);
    }

    public Task<Notification> GetNotifications(string userId, CancellationToken token)
    {
        return FindNotification(userId, token);
    }

    public async Task SwitchPurchaces(string userId, CancellationToken token)
    {
        var notification = await FindNotification(userId, token);

        await _notifications
            .UpdateOneAsync(
            Builders<Notification>.Filter.Eq(x => x.UserId, userId),
            Builders<Notification>.Update.Set(x => x.Purchaces, !notification.Purchaces)
            );
    }

    public async Task SwitchReminders(string userId, CancellationToken token)
    {
        var notification = await FindNotification(userId, token);

        await _notifications
            .UpdateOneAsync(
            Builders<Notification>.Filter.Eq(x => x.UserId, userId),
            Builders<Notification>.Update.Set(x => x.Reminders, !notification.Reminders)
            );
    }

    private Task<Notification> FindNotification(string userId, CancellationToken token)
    {
        return _notifications
            .Find(Builders<Notification>.Filter.Eq(x => x.UserId, userId))
            .SingleOrDefaultAsync(token);
    }
}

