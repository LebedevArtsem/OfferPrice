namespace OfferPrice.Events.Interfaces;

public interface IEventResolver
{
    public EventOptions Resolve<T>() where T : Event;
}

