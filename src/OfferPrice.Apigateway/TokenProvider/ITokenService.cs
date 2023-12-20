using OfferPrice.Apigateway.TokenProvider.Models;

namespace OfferPrice.Apigateway.TokenProvider;

public interface ITokenService
{
    AccessToken GenerateAccessToken(string clientId, IEnumerable<string> roles);

    RefreshToken GenerateRefreshToken();
}

