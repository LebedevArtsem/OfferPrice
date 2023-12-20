using OfferPrice.Events.Models;

namespace OfferPrice.Events.Events;

public class UserUpdatedEvent : Event
{
    public UserUpdatedEvent(User user)
    {
        User = user;
    }

    public User User { get; set; }
}

