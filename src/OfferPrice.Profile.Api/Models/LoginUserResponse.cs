namespace OfferPrice.Profile.Api.Models;

public class LoginUserResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<string> Roles { get; set; }
}

