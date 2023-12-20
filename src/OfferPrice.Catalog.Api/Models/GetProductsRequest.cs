namespace OfferPrice.Catalog.Api.Models;
public class GetProductsRequest
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Category { get; set; }
}

