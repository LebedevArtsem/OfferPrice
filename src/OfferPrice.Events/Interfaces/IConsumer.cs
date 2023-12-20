namespace OfferPrice.Events.Interfaces;

public interface IConsumer : IDisposable
{
    void Consume(CancellationToken token = default);
}