using OfferPrice.Common;

namespace OfferPrice.Auction.Domain.Queries;

public class FindLotsQuery
{
    public string ProductOwnerId { get; set; }
    public string WinnerId { get; set; }
    public string[] Statuses { get; set; }
    public Paging Paging { get; set; }
}