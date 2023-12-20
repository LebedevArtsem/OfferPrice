using Microsoft.Extensions.Logging;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq;
using OfferPrice.Payment.Domain.Interfaces;
using OfferPrice.Payment.Domain.Models;

namespace OfferPrice.Payment.Infrastructure.Events;

public class UserCreatedEventConsumer : RabbitMqConsumer<UserCreatedEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserCreatedEventConsumer> _logger;

    public UserCreatedEventConsumer(
        IUserRepository userRepository,
        IEventResolver eventResolver,
        ILogger<UserCreatedEventConsumer> logger
    ) : base(eventResolver, logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    protected override async Task Execute(UserCreatedEvent message, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(message.User.Id, cancellationToken);
        if (user != null)
        {
            _logger.LogInformation("User {lot_id} is already exists", user.Id);
            return;
        }

        user = new User(message.User);
        await _userRepository.Create(user, cancellationToken);

        _logger.LogInformation("User {user} was successfully created", user);
    }
}
