using OfferPrice.Common;
using OfferPrice.Payment.Domain.Queries;

namespace OfferPrice.Payment.Api.Models;

public class TransactionsRequest
{
    public int Page { get; set; }

    public int PerPage { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public FindTransacitonQuery ToQuery(string clientId)
    {
        return new()
        {
            UserId = clientId,
            StartDate = StartDate,
            EndDate = EndDate,
            Paging = new Paging(Page, PerPage)
        };
    }
}

