using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OfferPrice.Apigateway.ConfigOptions;
using OfferPrice.Apigateway.TokenProvider;
using System.Text;

namespace OfferPrice.Apigateway.Extensions;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<TokenConfigOptions>(configuration.GetSection("TokenSettings"));

        var tokenConfig = configuration.GetSection("TokenSettings").Get<TokenConfigOptions>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var encodedKey = Encoding.ASCII.GetBytes(tokenConfig.Key);

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(encodedKey),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddSingleton<ITokenService, TokenService>();
    }
}

