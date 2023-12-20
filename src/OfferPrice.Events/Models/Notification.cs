using OfferPrice.Events.Enums;

namespace OfferPrice.Events.Models;

public class Notification
{
    public string UserId { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public NotificationType Type { get; set; }
}

