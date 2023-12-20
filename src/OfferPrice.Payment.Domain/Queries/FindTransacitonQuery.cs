using OfferPrice.Common;

namespace OfferPrice.Payment.Domain.Queries;

public class FindTransacitonQuery
{
    public string UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; }

    public Paging Paging { get; set; }
}

