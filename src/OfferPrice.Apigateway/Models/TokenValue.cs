namespace OfferPrice.Apigateway.Models;

public class TokenValue
{
    public DateTimeOffset ExpirationDateTime { get; set; }

    public string ClientId { get; set; }

    public List<string> Roles { get; set; }

    public TokenValue(string clientId, List<string> roles, DateTimeOffset expirationDateTime)
    {
        ExpirationDateTime = expirationDateTime;
        ClientId = clientId;
        Roles = roles;
    }
}

