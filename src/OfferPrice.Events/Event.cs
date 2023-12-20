namespace OfferPrice.Events;

public class Event
{
    public Event()
    {
        Id = Guid.NewGuid().ToString();
        Timestamp = DateTime.UtcNow;
    }
    
    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
}