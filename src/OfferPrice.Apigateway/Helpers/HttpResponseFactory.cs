using OfferPrice.Apigateway.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OfferPrice.Apigateway.Helpers;

public static class HttpResponseFactory
{
    public static HttpResponseMessage CreateResponseWithTokens(string accessToken, string refreshToken)
    {
        var authenticationResponse = new AuthenticationResponse(accessToken, refreshToken);
        var tokensJson = JsonSerializer.Serialize(authenticationResponse);
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        response.Content = new StringContent(tokensJson);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return response;
    }
}

