using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly IRunnerFactory _runnerFactory;
  private readonly IFactory<RegisterUserAction> _registerUserActionFactory;
  private readonly IFactory<SendConfirmationRequestEmailAction> _sendConfirmationRequestEmailActionFactory;
  private readonly IFactory<ConfirmEmailAction> _confirmEmailActionFactory;
  public UserRegistration(
    IRunnerFactory runnerFactory,
    IFactory<RegisterUserAction> registerUserActionFactory,
    IFactory<SendConfirmationRequestEmailAction> sendConfirmationRequestEmailActionFactory,
    IFactory<ConfirmEmailAction> confirmEmailActionFactory)
  {
    _runnerFactory = runnerFactory;
    _registerUserActionFactory = registerUserActionFactory;
    _sendConfirmationRequestEmailActionFactory = sendConfirmationRequestEmailActionFactory;
    _confirmEmailActionFactory = confirmEmailActionFactory;
  }

  public async Task RegisterUser(
    RegistrationData registrationData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriter2Runner(
      _registerUserActionFactory.Create(),
      registrationRes => new SendConfirmationRequestDto(
        createEmailLink((registrationRes.Token, registrationData.Email)), 
        registrationData.Name, 
        registrationData.Email),
      _sendConfirmationRequestEmailActionFactory.Create());
    await runner.Run(registrationData, ct);
  }

  public async Task ConfirmEmailAsync(string email, string token, CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriterRunner(
      _confirmEmailActionFactory.Create());
    await runner.Run(new(email, token), ct);
  }
}
