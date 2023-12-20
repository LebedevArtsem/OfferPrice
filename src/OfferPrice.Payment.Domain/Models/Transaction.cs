namespace OfferPrice.Payment.Domain.Models;

public class Transaction
{
    public string Id { get; set; }

    public string Rnn { get; set; }

    public string AuthCode { get; set; }

    public string UserId { get; set; }

    public string AuctionId { get; set; }

    public decimal Price { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; }
}

