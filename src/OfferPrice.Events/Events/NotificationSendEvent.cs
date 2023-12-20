using OfferPrice.Events.Models;

namespace OfferPrice.Events.Events;

public class NotificationSendEvent : Event
{
    public NotificationSendEvent(Notification notification)
    {
        Notification = notification;
    }

    public Notification Notification { get; set; }
}

