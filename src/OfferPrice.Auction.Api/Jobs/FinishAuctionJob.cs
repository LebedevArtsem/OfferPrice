using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using OfferPrice.Auction.Api.Hubs;
using OfferPrice.Auction.Api.Models;
using OfferPrice.Auction.Api.Settings;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Jobs;

public class FinishAuctionJob : BackgroundService
{
    private readonly ILotRepository _lotRepository;
    private readonly IHubContext<AuctionHub> _hubContext;
    private readonly IProducer _producer;
    private readonly AuctionSettings _settings;

    public FinishAuctionJob(ILotRepository lotRepository, IHubContext<AuctionHub> hubContext, IProducer producer, AuctionSettings settings)
    {
        _lotRepository = lotRepository;
        _hubContext = hubContext;
        _producer = producer;
        _settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Handle(stoppingToken);
            await Task.Delay(_settings.PauseInMs, stoppingToken);
        }
    }

    private async Task Handle(CancellationToken cancellationToken)
    {
        var lots = await _lotRepository.GetNonFinished(DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(_settings.BetIntervalInSec)), cancellationToken); // todo: make batches

        foreach (var lot in lots)
        {
            lot.Finish();

            try
            {
                await _lotRepository.Update(lot, cancellationToken); // todo: add concurrency
                await _hubContext.Clients.Group(lot.Id).SendAsync(
                    nameof(IAuctionClient.OnAuctionFinished),
                    new AuctionFinishedResponse(lot),
                    cancellationToken
                );

                var lotTask = new Task(() =>
                {
                    _producer.SendMessage(new LotStatusUpdatedEvent
                    {
                        LotId = lot.Id,
                        ProductId = lot.Product.Id,
                        Status = lot.Status
                    });
                });

                var notificationTask = lot.Winner is null ?
                    new Task(() =>
                    {
                        _producer.SendMessage(
                        new NotificationSendEvent(
                            new()
                            {
                                UserId = lot.Winner.Id,
                                Subject = "You are win the Lot!",
                                Body = "Pay for the product)",
                                Type = Events.Enums.NotificationType.Reminder
                            }
                            ));
                    }) :
                    Task.CompletedTask;

                await Task.WhenAll(lotTask, notificationTask);
            }
            catch
            {
                // todo: add concurrency
            }
        }
    }
}