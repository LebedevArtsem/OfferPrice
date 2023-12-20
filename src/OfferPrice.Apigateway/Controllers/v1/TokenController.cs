using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using OfferPrice.Apigateway.Exceptions;
using OfferPrice.Apigateway.Models;
using OfferPrice.Apigateway.TokenProvider;
namespace OfferPrice.Apigateway.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth/token")]
public class TokenController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly ITokenService _tokenGenerator;
    public TokenController(
        IMemoryCache memoryCache,
        ITokenService tokenGenerator)
    {
        _memoryCache = memoryCache;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("access")]
    public IActionResult GetNewAccesTokenByRefreshToken([FromBody] string oldRefreshToken)
    {
        if (!_memoryCache.TryGetValue(oldRefreshToken, out TokenValue tokenValue))
            return Unauthorized("RefreshToken is empty");

        if (tokenValue.ExpirationDateTime < DateTimeOffset.UtcNow)
            return Unauthorized("DateTime is expired");

        var accessToken = _tokenGenerator.GenerateAccessToken(tokenValue.ClientId, tokenValue.Roles);
        if (accessToken?.Token is null)
            throw new TokenException("Access token is null");

        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

        var result = new AuthenticationResponse(accessToken.Token, newRefreshToken.Token);

        _memoryCache.Remove(oldRefreshToken);

        var newTokenValue = new TokenValue(tokenValue.ClientId, tokenValue.Roles, newRefreshToken.ExpirationDate);
        _memoryCache.Set(newRefreshToken.Token, newTokenValue);

        return Ok(result);
    }
}

