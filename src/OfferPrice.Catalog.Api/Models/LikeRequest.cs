using System.ComponentModel.DataAnnotations;

namespace OfferPrice.Catalog.Api.Models;

public class LikeRequest
{
    [Required]
    public string UserId { get; set; }
}