namespace OfferPrice.Common;

public class Paging
{
    public Paging(int page, int perPage)
    {
        Page = page;
        PerPage = perPage;
    }

    public int Page { get; set; }
    public int PerPage { get; set; }
}