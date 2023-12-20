using OfferPrice.Common.Email.Models;

namespace OfferPrice.Common.Email;

public interface IEmailProviderService
{
    Task SendEmail(string emailReciever, MailContent content);
}

