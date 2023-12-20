using OfferPrice.Auction.Api.Models;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Hubs;

public interface IAuctionClient
{
    Task OnBetRaised(RaiseBetResponse response);
    Task OnBetDeclined(RaiseBetResponse response);
    Task OnAuctionFinished(AuctionFinishedResponse response);
    Task OnAuctionNotStarted();
    Task OnAuctionStarted(AuctionStartedResponse response);
    Task OnAuctionNotExist();
}