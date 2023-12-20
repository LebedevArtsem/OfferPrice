namespace OfferPrice.Events.Events;

public class LotStatusUpdatedEvent : Event
{
    public string LotId { get; set; }
    public string ProductId { get; set; }
    public string Status { get; set; }
}