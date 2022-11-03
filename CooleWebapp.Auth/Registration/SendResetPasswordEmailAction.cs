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
          dataIn.Link),
        ct);
      return Unit.Default;
    }
  }
}
