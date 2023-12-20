namespace OfferPrice.Apigateway.TokenProvider.Models;

public class RefreshToken
{
    public string Token { get; set; }

    public DateTimeOffset ExpirationDate { get; set; }
}

