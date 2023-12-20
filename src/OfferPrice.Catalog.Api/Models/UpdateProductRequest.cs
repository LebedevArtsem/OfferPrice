using System.ComponentModel.DataAnnotations;

namespace OfferPrice.Catalog.Api.Models
{
    public class UpdateProductRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Category { get; set; }

        public string Brand { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Invalid minimal price")]
        public decimal Price { get; set; }
    }
}
