using Microsoft.AspNetCore.SignalR;
using OfferPrice.Auction.Api.Models;
using OfferPrice.Auction.Api.Settings;
using OfferPrice.Auction.Domain.Interfaces;
using SignalRSwaggerGen.Attributes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Hubs;

[SignalRHub("/hubs/auction")]
public class AuctionHub : Hub<IAuctionClient>
{
    private readonly ILotRepository _lotRepository;
    private readonly IUserRepository _userRepository;
    private readonly AuctionSettings _settings;

    public AuctionHub(ILotRepository lotRepository, IUserRepository userRepository, AuctionSettings settings)
    {
        _lotRepository = lotRepository;
        _userRepository = userRepository;
        _settings = settings;
    }

    public async Task RaiseBet(RaiseBetRequest request)
    {
        var cancellationToken = CancellationToken.None;

        var auctionId = GetAuctionId();
        var lot = await _lotRepository.Get(auctionId, cancellationToken);

        if (lot == null)
        {
            await Clients.Caller.OnAuctionNotExist();
            return;
        }

        if (lot.IsFinished())
        {
            await Clients.Caller.OnAuctionFinished(new(lot));
            return;
        }

        if (!lot.IsStarted())
        {
            await Clients.Caller.OnAuctionNotStarted();
            return;
        }

        var user = await _userRepository.Get(request.UserId, cancellationToken);
        if (user == null)
        {
            await Clients.Caller.OnBetDeclined(new(lot, lot.Updated.AddSeconds(_settings.BetIntervalInSec)));
            return;
        }

        var _ = lot.RaiseBet(user, request.Raise);
        try
        {
            await _lotRepository.Update(lot, cancellationToken);
        }
        catch
        {
            // todo: handle concurrency
            await Clients.Caller.OnBetDeclined(new(lot, lot.Updated.AddSeconds(_settings.BetIntervalInSec))); // todo: fix
            return;
        }

        await Clients.Group(auctionId).OnBetRaised(new(lot, lot.Updated.AddSeconds(_settings.BetIntervalInSec)));
    }

    public override async Task OnConnectedAsync()
    {
        var auctionId = GetAuctionId();
        var lot = await _lotRepository.Get(auctionId, CancellationToken.None);

        if (lot == null)
        {
            await Clients.Caller.OnAuctionNotExist();
        }
        else if (lot.IsStarted())
        {
            await Clients.Caller.OnAuctionStarted(new AuctionStartedResponse(lot.Price, lot.Updated.AddSeconds(_settings.BetIntervalInSec)));
        }
        else if (lot.IsFinished())
        {
            await Clients.Caller.OnAuctionFinished(new AuctionFinishedResponse(lot));
        }
        else
        {
            await Clients.Caller.OnAuctionNotStarted();
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var auctionId = GetAuctionId();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, auctionId);
        await base.OnDisconnectedAsync(exception);
    }

    private string GetAuctionId()
    {
        return Context.GetHttpContext()?.Request.Headers["Auction-Id"];
    }
}