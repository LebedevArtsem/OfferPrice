namespace OfferPrice.Auction.Api.Models;

public class User
{
    public string Id { get; set; }
    public string Email { get; set; }

    public static User FromDomain(Domain.Models.User user)
    {
        if (user == null)
        {
            return null;
        }
        
        return new()
        {
            Id = user.Id,
            Email = user.Email
        };
    }
}