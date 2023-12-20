using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Common.Email;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Profile.Api.Models;
using OfferPrice.Profile.Domain.Interfaces;
using OfferPrice.Profile.Domain.Models;
using OfferPrice.Profile.Infrastructure.Helpers;

namespace OfferPrice.Profile.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth/")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IDoubleAuthRepository _doubleAuthRepository;
    private readonly IEmailProviderService _emailProviderService;
    private readonly IProducer _producer;

    public AuthController(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        INotificationRepository notificationRepository,
        IDoubleAuthRepository doubleAuthRepository,
        IEmailProviderService emailProviderService,
        IProducer producer,
        IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _notificationRepository = notificationRepository;
        _doubleAuthRepository = doubleAuthRepository;
        _emailProviderService = emailProviderService;
        _producer = producer;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken token)
    {
        //var userEntity = await _userRepository.GetByEmailAndPassword(request.Email, request.Password, token);

        var userEntity = await _userRepository.GetByEmail(request.Email, token);

        var hasher = new PasswordHasher<Domain.Models.User>();

        var validPassword = hasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, request.Password);

        if (userEntity.PasswordHash == request.Password)
        {
            return BadRequest(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400));
        }

        //var userResponse = _mapper.Map<LoginUserResponse>(userEntity);

        var doubleAuthEntity = new DoubleAuth()
        {
            Id = Guid.NewGuid().ToString(),
            UserEmail = userEntity.Email,
            Code = DoubleAuthCodeGenerator.Generate(4),
            CreationDate = DateTime.Now
        };

        await _doubleAuthRepository.Create(doubleAuthEntity, token);

        await _emailProviderService.SendEmail(request.Email, new()
        {
            Subject = "Double authentication",
            Body = $"Code: {doubleAuthEntity.Code}"
        });

        return Ok();
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationUserRequest createUserRequest, CancellationToken token)
    {
        var user = _mapper.Map<Domain.Models.User>(createUserRequest);

        var hasher = new PasswordHasher<Domain.Models.User>();

        user.PasswordHash = hasher.HashPassword(user, createUserRequest.Password);

        var role = await _roleRepository.GetByName("user", token);

        user.Roles = new List<Role> { role };

        await Task.WhenAll(
            _userRepository.Create(user, token),
            _notificationRepository.CreateNotifications(user.Id, token));

        _producer.SendMessage(new UserCreatedEvent(user.ToEvent()));

        return Ok();
    }

    [HttpPost("double_authentication")]
    public async Task<IActionResult> CheckDoubleAuthentication([FromBody] DoubleAuthRequest doubleAuth, CancellationToken cancellationToken)
    {
        var doubleAuthEnity = await _doubleAuthRepository.GetByUserEmail(doubleAuth.UserEmail, cancellationToken);

        if (doubleAuthEnity is null || doubleAuthEnity.Code != doubleAuth.Code)
        {
            return BadRequest();
        }

        var userEntity = await _userRepository.GetByEmail(doubleAuth.UserEmail, cancellationToken);

        var userResponse = _mapper.Map<LoginUserResponse>(userEntity);

        return Ok(userResponse);
    }

    [HttpPatch("double_authentication")]
    public async Task<IActionResult> UpdateDoubleAuthentication([FromBody] string userEmail, CancellationToken cancellationToken)
    {
        var doubleAuthEntity = await _doubleAuthRepository.GetByUserEmail(userEmail, cancellationToken);

        doubleAuthEntity.Code = DoubleAuthCodeGenerator.Generate(4);
        doubleAuthEntity.CreationDate = DateTime.Now;

        await _doubleAuthRepository.Update(doubleAuthEntity, cancellationToken);

        await _emailProviderService.SendEmail(userEmail, new()
        {
            Subject = "Double authentication",
            Body = $"Code: {doubleAuthEntity.Code}"
        });

        return Ok();
    }
}

