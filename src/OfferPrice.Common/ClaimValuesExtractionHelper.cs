using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OfferPrice.Common;

public static class ClaimValuesExtractionHelper
{
    public static string GetClientIdFromUserClaimIn(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        return httpContext.User.GetClaimValue("clientId");
    }

    public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.GetClaim(claimType).Value;
    }

    public static Claim GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType);

        if (claim == null)
        {
            throw new Exception(claimType); //TODO: create custom exception
        }

        return claim;
    }
}

