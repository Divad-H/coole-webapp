namespace CooleWebapp.Application.EmailService;

public interface IEmailSender
{
  Task SendEmailAsync(Message message, CancellationToken ct);
}
