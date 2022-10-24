using CooleWebapp.Application.EmailService;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CooleWebapp.EmailService;

internal sealed class EmailSender : IEmailSender
{
  private readonly EmailConfiguration _emailConfig;
  private readonly ILogger<EmailSender> _logger;

  public EmailSender(
    IOptions<EmailConfiguration> emailConfig,
    ILogger<EmailSender> logger)
    => (_emailConfig, _logger) = (emailConfig.Value, logger);

  public async Task SendEmailAsync(Message message, CancellationToken ct)
  {
    var mailMessage = CreateEmailMessage(message);
    await SendAsync(mailMessage, ct);
  }

  private MimeMessage CreateEmailMessage(Message message)
  {
    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));
    emailMessage.To.AddRange(message.To.Select(to => new MailboxAddress(to.Name, to.Address)));
    emailMessage.Subject = message.Subject;

    var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

    emailMessage.Body = bodyBuilder.ToMessageBody();
    return emailMessage;
  }

  private async Task SendAsync(MimeMessage mailMessage, CancellationToken ct)
  {
    using var client = new SmtpClient();
    try
    {
      await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true, ct);
      client.AuthenticationMechanisms.Remove("XOAUTH2");
      await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password, ct);

      await client.SendAsync(mailMessage, ct);
    }
    catch (Exception ex)
    {
      _logger.LogError("Error while trying to send e-mail. {Exception}", ex.Message);
      throw;
    }
    finally
    {
      await client.DisconnectAsync(true, ct);
    }
  }
}
