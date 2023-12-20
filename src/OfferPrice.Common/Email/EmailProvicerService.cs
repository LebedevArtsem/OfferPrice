using Microsoft.Extensions.Options;
using OfferPrice.Common.Email.Models;
using OfferPrice.Common.Email.Options;
using System.Net.Mail;
using System.Net;

namespace OfferPrice.Common.Email;

public class EmailProvicerService : IEmailProviderService
{
    private readonly EmailOptions _emailOptions;

    public EmailProvicerService(IOptions<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public async Task SendEmail(string emailReciever, MailContent content)
    {
        var smtpServerOptions = _emailOptions.SmtpServer; // TODO: Delete appMail

        using var smtpClient = new SmtpClient(smtpServerOptions.ServerDomain, smtpServerOptions.Port);

        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(_emailOptions.SmtpServer.AppName, _emailOptions.SmtpServer.Password);
        smtpClient.EnableSsl = true;

        var message = new MailMessage(_emailOptions.AppMail.Address, emailReciever)
        {
            Subject = content.Subject,
            Body = content.Body
        };

        await smtpClient.SendMailAsync(message);
    }
}
