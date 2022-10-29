using CooleWebapp.Application.EmailService;
using CooleWebapp.Core.BusinessActionRunners;
using System.Reactive;

namespace CooleWebapp.Auth.Registration;

public class SendConfirmationRequestEmailAction : IBusinessAction<SendConfirmationRequestDto, Unit>
{
  private readonly IEmailSender _emailSender;

  public SendConfirmationRequestEmailAction(IEmailSender emailSender)
  {
    _emailSender = emailSender;
  }

  public async Task<Unit> Run(SendConfirmationRequestDto confirmationRequest, CancellationToken ct)
  {
    await _emailSender.SendEmailAsync(
      new Message(
        new (string Name, string Address)[]{
        (confirmationRequest.Name, Address: confirmationRequest.Email) },
        "Coole Webapp: Confirm E-Mail Address",
        $"Confirm your e-mail by following this link: {confirmationRequest.ConfirmationLink}"),
      ct);

    return Unit.Default;
  }
}
