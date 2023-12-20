using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Common;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Profile.Api.Models;
using OfferPrice.Profile.Domain.Interfaces;

namespace OfferPrice.Profile.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/users")]
[ApiController]
[ApiVersion("1.0")]
public class ProfileController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly IProducer _producer;
    private readonly IMapper _mapper;
    public ProfileController(IUserRepository users, IProducer producer, IMapper mapper)
    {
        _users = users;
        _producer = producer;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        var clientId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        var user = await _users.Get(clientId, token);

        if (user == null)
        {
            return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 404));
        }

        var userResponse = _mapper.Map<Models.User>(user);

        return Ok(userResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest, CancellationToken token)
    {
        var clientId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        var user = await _users.Get(clientId, token);

        if (user == null)
        {
            return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 404));
        }

        var update = _mapper.Map(updateUserRequest, user);
        await _users.Update(update, token);

        _producer.SendMessage(new UserUpdatedEvent(update.ToEvent()));

        return Ok();
    }
}

