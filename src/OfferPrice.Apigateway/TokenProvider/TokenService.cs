using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OfferPrice.Apigateway.ConfigOptions;
using OfferPrice.Apigateway.TokenProvider.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OfferPrice.Apigateway.TokenProvider;

public class TokenService : ITokenService
{
    private readonly TokenConfigOptions _tokenConfig;

    public TokenService(IOptions<TokenConfigOptions> tokenConfig)
    {
        _tokenConfig = tokenConfig.Value;
    }

    public AccessToken GenerateAccessToken(string clientId, IEnumerable<string> roles)
    {
        var tokenExpiredTime = DateTime.Now.AddMinutes(_tokenConfig.AccessTokenLifetime);
        var tokenKey = Encoding.ASCII.GetBytes(_tokenConfig.Key);


        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim("clientId", clientId)
        });

        foreach (var role in roles)
        {
            claimsIdentity.AddClaim(new Claim("role", role));
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature
            );

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiredTime,
            SigningCredentials = signingCredentials
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new AccessToken
        {
            Token = token
        };
    }

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            ExpirationDate = DateTime.UtcNow.AddMinutes(_tokenConfig.RefreshTokenLifetime)
        };
    }
}

