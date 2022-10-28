using CooleWebapp.Application.EmailService;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly IRunnerFactory _runnerFactory;
  private readonly IUserManager _userManager;
  private readonly IUserDataAccess _userDataAccess;
  private readonly IEmailSender _emailSender;
  public UserRegistration(
    IRunnerFactory runnerFactory,
    IUserManager userManager,
    IUserDataAccess userDataAccess,
    IEmailSender emailSender)
  {
    _runnerFactory = runnerFactory;
    _userManager = userManager;
    _userDataAccess = userDataAccess;
    _emailSender = emailSender;
  }

  public async Task RegisterUser(
    RegistrationData registrationData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct)
  {
    var runner = _runnerFactory.CreateTransactionRunner(
      new RegisterUserAction(createEmailLink, _userManager, _userDataAccess, _emailSender));
    await runner.Run(registrationData, ct);
  }

  public async Task ConfirmEmailAsync(string email, string token, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user is null)
      throw new ClientError(ErrorType.NotFound, "Invalid e-mail confirmation link. User not found.");

    await _userManager.ConfirmEmailAsync(user, token);
  }
}
