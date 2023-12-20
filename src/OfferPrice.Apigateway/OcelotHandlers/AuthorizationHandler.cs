using Microsoft.Extensions.Caching.Memory;
using OfferPrice.Apigateway.Exceptions;
using OfferPrice.Apigateway.Helpers;
using OfferPrice.Apigateway.Models;
using OfferPrice.Apigateway.TokenProvider;
using System.Net;
using System.Text.Json;

namespace OfferPrice.Apigateway.OcelotHandlers;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly ITokenService _tokenGenerator;
    private readonly IMemoryCache _memoryCache;

    public AuthorizationHandler(
        ITokenService tokenGenerator,
        IMemoryCache memoryCache)
    {
        _tokenGenerator = tokenGenerator;
        _memoryCache = memoryCache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var userServiceResponse = await base.SendAsync(request, cancellationToken);

        if (userServiceResponse.StatusCode is not HttpStatusCode.OK)
            return userServiceResponse;

        var content = await userServiceResponse.Content.ReadAsStringAsync();
        var parsedContent = JsonDocument.Parse(content);
        var clientId = parsedContent.RootElement.GetProperty("id").GetString();
        if (clientId == null)
        {
            throw new TokenException("Client id is not found");
        }

        var roles = parsedContent.RootElement.GetProperty("roles").Deserialize<List<string>>();
        if (roles == null)
        {
            throw new TokenException("Client roles are not found");
        }

        var accessToken = _tokenGenerator.GenerateAccessToken(clientId, roles);
        if (accessToken is null)
        {
            throw new TokenException("Access token is not generated");
        }

        var refreshToken = _tokenGenerator.GenerateRefreshToken();
        if (refreshToken is null)
        {
            throw new TokenException("Refresh token is not generated");
        }

        var tokenValue = new TokenValue(clientId, roles, refreshToken.ExpirationDate);

        _memoryCache.Set(refreshToken.Token, tokenValue);

        return HttpResponseFactory.CreateResponseWithTokens(accessToken.Token, refreshToken.Token);
    }
}

