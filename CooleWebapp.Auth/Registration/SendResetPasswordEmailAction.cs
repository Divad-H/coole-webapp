using CooleWebapp.Application.EmailService;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Auth.Registration
{
  public class SendResetPasswordEmailAction : IBusinessAction<SendResetPasswordEmailDto, Unit>
  {
    private readonly IEmailSender _emailSender;
    private readonly IUserManager _userManager;
    private readonly IUserDataAccess _userDataAccess;
    public SendResetPasswordEmailAction(
      IEmailSender emailSender,
      IUserManager userManager,
      IUserDataAccess userDataAccess)
    {
      _emailSender = emailSender;
      _userManager = userManager;
      _userDataAccess = userDataAccess;
    }

    public async Task<Unit> Run(SendResetPasswordEmailDto dataIn, CancellationToken ct)
    {
      var user = await _userManager.FindByEmailAsync(dataIn.Email);
      if (user is null)
        throw new ClientError(ErrorType.NotFound, "A user with this e-mail was not found.");
      var coolUser = await _userDataAccess.FindUserByWebappUserId(user.Id, ct);
      if (coolUser is null)
        throw new InvalidOperationException("CoolUser was not found.");
      await _emailSender.SendEmailAsync(
        new Message(
          new[] { (Name: coolUser.Name, Address: dataIn.Email) }, 
          "Reset your Coole Webapp Password", 
          string.Format(@"<h1>Greetings human being called {0}*,</h1>
<p>you have requested to reset your password for the CooleWebApp, to do soplease click <a href=""{1}"">here.</a></p>
<p>If you didn't request it, please ignore this email. In case of multiple of unrequested password reset emails, please contact our <a href=""mailto:oliver.delpy@procom.de"">support</a>.</p>
<hr />
<p><sub>*: We are terribly sorry for assuming you have a name.</sub></p>
", coolUser.Name, dataIn.Link)),
        ct);
      return Unit.Default;
    }
  }
}
