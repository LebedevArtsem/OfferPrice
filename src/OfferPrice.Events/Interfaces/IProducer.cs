namespace OfferPrice.Events.Interfaces;

public interface IProducer
{
    void SendMessage<T>(T message) where T : Event;
}