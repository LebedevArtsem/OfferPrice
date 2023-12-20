using System.ComponentModel.DataAnnotations;

namespace OfferPrice.Profile.Api.Models;
public class CreateUserRequest
{
    [Required]
    public string Id { get; set; } // todo: remove when custom authorization appears
    public string Name { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

