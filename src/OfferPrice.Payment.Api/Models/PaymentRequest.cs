namespace OfferPrice.Payment.Api.Models;

public class PaymentRequest
{
    public string AuctionId { get; set; }

    public string CardNumber { get; set; }

    public int CardPin { get; set; }

    public DateTime CardExpirationDate { get; set; }

    public string Fio { get; set; }

    public decimal Price { get; set; }
}

