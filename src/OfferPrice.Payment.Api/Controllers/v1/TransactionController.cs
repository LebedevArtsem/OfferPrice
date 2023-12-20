using Microsoft.AspNetCore.Mvc;
using OfferPrice.Common;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;
using OfferPrice.Payment.Api.Models;
using OfferPrice.Payment.Domain.Interfaces;
using OfferPrice.Payment.Domain.Models;

namespace OfferPrice.Payment.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/transaction/")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProducer _producer;

    public TransactionController(
        ITransactionRepository transactionRepository,
        IProducer producer)
    {
        _transactionRepository = transactionRepository;
        _producer = producer;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserTransactions([FromQuery] TransactionsRequest request, CancellationToken token)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        var result = await _transactionRepository.Find(request.ToQuery(userId), token);

        return Ok(result);
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PaymentRequest request, CancellationToken token)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        //Create a response to WebMoney

        var transaction = new Transaction()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            AuctionId = request.AuctionId,
            AuthCode = "123456789012",
            Rnn = "123456",
            Date = DateTime.Now,
            Status = "Done",
            Price = request.Price
        };

        await _transactionRepository.Create(transaction, token);

        _producer.SendMessage(
            new NotificationSendEvent(
                new()
                {
                    UserId = userId,
                    Subject = "OfferPrice purchase",
                    Body = "You have paid for the lot",
                    Type = Events.Enums.NotificationType.Purchase
                }
            ));

        return Ok();
    }
}


