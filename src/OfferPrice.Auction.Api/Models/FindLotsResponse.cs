using OfferPrice.Common;
using System.Collections.Generic;
using System.Linq;

namespace OfferPrice.Auction.Api.Models;

public class FindLotsResponse
{
    public FindLotsResponse(PageResult<Domain.Models.Lot> pageResult)
    {
        Lots = pageResult.Items.Select(x => new Lot(x)).ToList();
        Total = pageResult.Total;
    }   
    
    public List<Lot> Lots { get; set; }
    public long Total { get; set; }
}