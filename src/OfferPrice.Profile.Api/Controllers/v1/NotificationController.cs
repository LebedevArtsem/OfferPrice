using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Common;
using OfferPrice.Profile.Api.Models;
using OfferPrice.Profile.Domain.Interfaces;

namespace OfferPrice.Profile.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/notifications")]
[ApiController]
[ApiVersion("1.0")]
public class NotificationController : ControllerBase
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationController(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(CancellationToken token)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        var notifications = await _notificationRepository.GetNotifications(userId, token);

        var response = _mapper.Map<NotificationsResponse>(notifications);

        return Ok(response);
    }

    [HttpPut("reminder")]
    public async Task<IActionResult> SwitchReminder(CancellationToken token)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        await _notificationRepository.SwitchReminders(userId, token);

        return Ok();
    }
    
    [HttpPut("purchase")]
    public async Task<IActionResult> SwitchPurchase(CancellationToken token)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        await _notificationRepository.SwitchPurchaces(userId, token);

        return Ok();
    }
}
