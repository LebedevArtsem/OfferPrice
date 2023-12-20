using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Events;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Profile.Api.Models;
using OfferPrice.Profile.Domain.Interfaces;

namespace OfferPrice.Profile.Api.Controllers;

[Route("api/users")]
[ApiController]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken token)
    {
        var user = await _users.Get(id, token);

        if (user == null)
        {
            return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 404));
        }

        var userResponse = _mapper.Map<Models.User>(user);

        return Ok(userResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest createUserRequest, CancellationToken token)
    {
        var user = _mapper.Map<Domain.Models.User>(createUserRequest);

        await _users.Create(user, token);
        
        _producer.SendMessage(new UserCreatedEvent(user.ToEvent()));

        var userResponse = _mapper.Map<Models.User>(user);

        return Ok(userResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest updateUserRequest, CancellationToken token)
    {
        var user = await _users.Get(id, token);

        if (user == null)
        {
            return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 404));
        }

        var update = _mapper.Map(updateUserRequest, user);
        await _users.Update(update, token);

        return Ok();
    }
}

