using OfferPrice.Auction.Domain.Queries;
using OfferPrice.Common;
using System;

namespace OfferPrice.Auction.Api.Models;

public class FindLotsRequest
{
    public string ProductOwnerId { get; set; }
    public string WinnerId { get; set; }
    public string[] Statuses { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }

    public FindLotsQuery ToQuery()
    {
        return new()
        {
            ProductOwnerId = ProductOwnerId,
            WinnerId = WinnerId,
            Statuses = Statuses ?? Array.Empty<string>(),
            Paging = new Paging(Page, PerPage)
        };
    }
}