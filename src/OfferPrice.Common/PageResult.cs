namespace OfferPrice.Common;
public class PageResult<T>
{
    public int Page { get; set; }

    public int PerPage { get; set; }

    public long Total { get; set; }

    public List<T> Items { get; set; }

    public PageResult(int page, int perPage, long total, List<T> items)
    {
        Page = page;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}

