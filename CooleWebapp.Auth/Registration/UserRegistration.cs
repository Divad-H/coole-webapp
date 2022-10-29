using CooleWebapp.Application.EmailService;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;

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
    var runner = _runnerFactory.CreateTransaction2Runner(
      new RegisterUserAction(_userManager, _userDataAccess),
      registrationRes => new SendConfirmationRequestDto(
        createEmailLink((registrationRes.Token, registrationData.Email)), 
        registrationData.Name, 
        registrationData.Email),
      new SendConfirmationRequestEmailAction(_emailSender));
    await runner.Run(registrationData, ct);
  }

  public async Task ConfirmEmailAsync(string email, string token, CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriterRunner(new ConfirmEmailAction(_userManager));
    await runner.Run(new(email, token), ct);
  }
}
