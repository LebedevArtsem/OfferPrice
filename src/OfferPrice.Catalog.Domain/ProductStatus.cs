namespace OfferPrice.Catalog.Domain;

public static class ProductStatus
{
    public const string Created = "Created";
    public const string ReadyToPlay = "ReadyToPlay";
    public const string InPlay = "InPlay";
    public const string PlayedOut = "PlayedOut";

    public static string GetFromLotStatus(string lotStatus)
    {
        return lotStatus switch
        {
            LotStatus.Planned => ReadyToPlay,
            LotStatus.Started => InPlay,
            LotStatus.Sold => PlayedOut,
            LotStatus.Unsold => PlayedOut,
            _ => null,
        };
    }
}