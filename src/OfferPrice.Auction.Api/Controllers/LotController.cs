using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Auction.Api.Models;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Common.Extensions;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Controllers;
[ApiController]
[Route("api/lots")]
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
}
