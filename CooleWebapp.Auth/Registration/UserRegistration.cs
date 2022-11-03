using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly IRunnerFactory _runnerFactory;
  private readonly IFactory<RegisterUserAction> _registerUserActionFactory;
  private readonly IFactory<SendConfirmationRequestEmailAction> _sendConfirmationRequestEmailActionFactory;
  private readonly IFactory<ConfirmEmailAction> _confirmEmailActionFactory;
  private readonly IFactory<SendResetPasswordEmailAction> _sendResetPasswordEmailActionFactory;
  private readonly IFactory<StartInitiatePasswordResetAction> _startResetPasswordActionFactory;
  public UserRegistration(
    IRunnerFactory runnerFactory,
    IFactory<RegisterUserAction> registerUserActionFactory,
    IFactory<SendConfirmationRequestEmailAction> sendConfirmationRequestEmailActionFactory,
    IFactory<ConfirmEmailAction> confirmEmailActionFactory,
    IFactory<SendResetPasswordEmailAction> sendResetPasswordEmailActionFactory,
    IFactory<StartInitiatePasswordResetAction> startResetPasswordActionFactory)
  {
    _runnerFactory = runnerFactory;
    _registerUserActionFactory = registerUserActionFactory;
    _sendConfirmationRequestEmailActionFactory = sendConfirmationRequestEmailActionFactory;
    _confirmEmailActionFactory = confirmEmailActionFactory;
    _sendResetPasswordEmailActionFactory = sendResetPasswordEmailActionFactory;
    _startResetPasswordActionFactory = startResetPasswordActionFactory;
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

  public async Task InitiatePasswordReset(
    StartInitiatePasswordResetDto initiatePasswordResetDto,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriter2Runner(
      _startResetPasswordActionFactory.Create(),
      token => new SendResetPasswordEmailDto(
        createEmailLink((token, initiatePasswordResetDto.Email)),
        initiatePasswordResetDto.Email),
      _sendResetPasswordEmailActionFactory.Create());
    await runner.Run(initiatePasswordResetDto, ct);
  }
}
