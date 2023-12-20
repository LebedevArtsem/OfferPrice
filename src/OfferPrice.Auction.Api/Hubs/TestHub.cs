using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Hubs;

public class TestHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("RecievedMessage", "asdfasdfasdf");
    }
}

