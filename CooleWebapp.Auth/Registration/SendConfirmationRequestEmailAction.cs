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
        String.Format(@"<h1>Greetings human being called {0}*,</h1>
<p>welcome to the CooleWebApp, to complete your registration please click <a href=""{1}"">here.</a></p>
<hr />
<p><sub>*: We are terribly sorry for assuming you have a name.</sub></p>
", confirmationRequest.Name, confirmationRequest.ConfirmationLink)),
      ct);

    return Unit.Default;
  }
}
