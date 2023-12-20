namespace OfferPrice.Apigateway.ConfigOptions;

public class TokenConfigOptions
{
    public string Key { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int AccessTokenLifetime { get; set; }

    public int RefreshTokenLifetime { get; set; }
}

