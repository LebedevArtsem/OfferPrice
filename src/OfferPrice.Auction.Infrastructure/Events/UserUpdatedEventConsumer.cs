using Microsoft.Extensions.Logging;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Auction.Domain.Models;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Events.RabbitMq;

namespace OfferPrice.Auction.Infrastructure.Events;

public class UserUpdatedEventConsumer : RabbitMqConsumer<UserUpdatedEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserUpdatedEventConsumer> _logger;

    public UserUpdatedEventConsumer(
        IUserRepository userRepository,
        IEventResolver eventResolver,
        ILogger<UserUpdatedEventConsumer> logger
        ) : base(eventResolver, logger)
    {
        _userRepository = userRepository;
        _logger = logger; 
    }

    protected override async Task Execute(UserUpdatedEvent message, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(message.User.Id, cancellationToken);
        if (user is null)
        {
            _logger.LogInformation("User isn't exists");
            return;
        }

        user = new User(message.User);
        await _userRepository.Update(user, cancellationToken);

        _logger.LogInformation("User {user} was successfully updated", user);
    }
}

