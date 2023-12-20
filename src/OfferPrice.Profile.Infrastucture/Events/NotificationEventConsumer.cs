using Microsoft.Extensions.Logging;
using OfferPrice.Common.Email;
using OfferPrice.Events.Enums;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq;
using OfferPrice.Profile.Domain.Interfaces;

namespace OfferPrice.Profile.Infrastructure.Events;

public class NotificationEventConsumer : RabbitMqConsumer<NotificationSendEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailProviderService _emailProviderService;
    private readonly ILogger<NotificationEventConsumer> _logger;

    public NotificationEventConsumer(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IEventResolver eventResolver,
        ILogger<NotificationEventConsumer> logger)
        : base(eventResolver, logger)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    protected override async Task Execute(NotificationSendEvent message, CancellationToken cancellationToken)
    {
        var userNotification = await _notificationRepository.GetNotifications(message.Notification.UserId, cancellationToken);

        if (userNotification is null)
        {
            _logger.LogInformation("Notifications are not found");
            return;
        }

        var user = await _userRepository.Get(message.Notification.UserId, cancellationToken);

        var shouldBeSend = true;

        switch (message.Notification.Type)
        {
            case NotificationType.Purchase:
                shouldBeSend = userNotification.Purchaces == true ? true : false;
                break;

            case NotificationType.Reminder:
                shouldBeSend = userNotification.Reminders == true ? true : false;
                break;
        }

        if (shouldBeSend)
        {
            _logger.LogInformation($"Sending notification to email:{user.Email}");

            await _emailProviderService.SendEmail(
                user.Email,
                new()
                {
                    Subject = message.Notification.Subject,
                    Body = message.Notification.Body
                });
        }
    }
}

