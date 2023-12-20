using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Auction.Api.Models;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Common.Extensions;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/lots")]
public class LotController : ControllerBase
{
    private readonly ILotRepository _lots;
    private readonly IUserRepository _users;
    private readonly IProducer _producer;

    private readonly IMapper _mapper;

    public LotController(ILotRepository lots, IUserRepository users, IProducer producer, IMapper mapper)
    {
        _lots = lots;
        _users = users;
        _producer = producer;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FindLotsRequest request, CancellationToken token)
    {
        var result = await _lots.Find(request.ToQuery(), token);

        return Ok(new FindLotsResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken token)
    {
        var lot = await _lots.Get(id, token);

        if (lot == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<Models.Lot>(lot));
    }

    [HttpPost("{id}/raise_bet")]
    public async Task<IActionResult> RaiseBet([FromBody] RaiseBetRequest raiseBetRequest, [FromRoute] string id, CancellationToken cancellationToken)
    {
        var auctionId = id;
        var lot = await _lots.Get(auctionId, cancellationToken);

        if (lot == null)
        {
            return BadRequest();
        }

        if (lot.IsFinished())
        {
            return Conflict();
        }

        if (!lot.IsStarted())
        {
            return BadRequest();
        }

        var user = await _users.Get(raiseBetRequest.UserId, cancellationToken);
        if (user == null)
        {
            return NotFound();
        }

        var _ = lot.RaiseBet(user, raiseBetRequest.Raise);
        try
        {
            await _lots.Update(lot, cancellationToken);
        }
        catch
        {
            // todo: handle concurrency

        }

        return Ok();
    }

    [HttpPost("{id}/delivery")]
    public async Task<IActionResult> Deliver([FromRoute] string id, CancellationToken token)
    {
        var lot = await _lots.Get(id, token);

        if (lot == null)
        {
            return NotFound();
        }

        if (!lot.IsSold())
        {
            return Conflict();
        }

        lot.Deliver();
        await _lots.Update(lot, token);

        _producer.SendMessage(new LotStatusUpdatedEvent
        {
            LotId = lot.Id,
            ProductId = lot.Product.Id,
            Status = lot.Status
        });

        return Ok();
    }

    [HttpPost("{id}/schedule")]
    public async Task<IActionResult> Schedule(string id, [FromBody] ScheduleLotRequest request, CancellationToken cancellationToken)
    {
        var lot = await _lots.Get(id, cancellationToken);
        if (lot == null)
        {
            return NotFound();
        }

        var user = await _users.Get(request.UserId, cancellationToken);
        if (user == null)
        {
            return Conflict();
        }

        if (lot.Product.User.Id != request.UserId)
        {
            return Conflict();
        }

        if (lot.IsStarted())
        {
            return Conflict();
        }

        lot.Schedule(request.Start.RemoveSeconds());
        await _lots.Update(lot, cancellationToken);

        _producer.SendMessage(new LotStatusUpdatedEvent
        {
            LotId = lot.Id,
            ProductId = lot.Product.Id,
            Status = lot.Status
        });

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction([FromRoute] string id, CancellationToken token)
    {
        await _lots.Delete(id, token);

        return Ok();
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartAuction(string id, CancellationToken cancellationToken)
    {
        var lot = await _lots.Get(id, cancellationToken);
        lot.Begin();

        try
        {
            await _lots.Update(lot, cancellationToken); // todo: add concurrency

            var lotTask = new Task(() =>
            {
                _producer.SendMessage(new LotStatusUpdatedEvent
                {
                    LotId = lot.Id,
                    ProductId = lot.Product.Id,
                    Status = lot.Status
                });
            });

            var notificationTask = new Task(() =>
            {
                //_producer.SendMessage(
                //new NotificationSendEvent(
                //    new() // TODO: make here a factory to construct massage
                //    {
                //        UserId = lot.Winner.Id,
                //        Subject = "OfferPrice",
                //        Body = "Lot is starting now! Harry up!",
                //        Type = Events.Enums.NotificationType.Reminder
                //    }
                //    ));
            });

            await Task.WhenAll(lotTask, notificationTask);


        }
        catch { }

        return Ok();
    }
}
