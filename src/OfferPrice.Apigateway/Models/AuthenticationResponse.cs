namespace OfferPrice.Apigateway.Models;

public class AuthenticationResponse
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public AuthenticationResponse(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}

