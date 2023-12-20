using OfferPrice.Events.Models;

namespace OfferPrice.Events.Events;

public class UserCreatedEvent : Event
{
    public UserCreatedEvent(User user)
    {
        User = user;
    }

    public User User { get; set; }
}