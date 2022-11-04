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
  private readonly IFactory<ResetPasswordAction> _resetPasswordActionFactory;
  public UserRegistration(
    IRunnerFactory runnerFactory,
    IFactory<RegisterUserAction> registerUserActionFactory,
    IFactory<SendConfirmationRequestEmailAction> sendConfirmationRequestEmailActionFactory,
    IFactory<ConfirmEmailAction> confirmEmailActionFactory,
    IFactory<SendResetPasswordEmailAction> sendResetPasswordEmailActionFactory,
    IFactory<StartInitiatePasswordResetAction> startResetPasswordActionFactory,
    IFactory<ResetPasswordAction> resetPasswordActionFactory)
  {
    _runnerFactory = runnerFactory;
    _registerUserActionFactory = registerUserActionFactory;
    _sendConfirmationRequestEmailActionFactory = sendConfirmationRequestEmailActionFactory;
    _confirmEmailActionFactory = confirmEmailActionFactory;
    _sendResetPasswordEmailActionFactory = sendResetPasswordEmailActionFactory;
    _startResetPasswordActionFactory = startResetPasswordActionFactory;
    _resetPasswordActionFactory = resetPasswordActionFactory;
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
    InitiatePasswordResetData initiatePasswordResetData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriter2Runner(
      _startResetPasswordActionFactory.Create(),
      token => new SendResetPasswordEmailDto(
        createEmailLink((token, initiatePasswordResetData.Email)),
        initiatePasswordResetData.Email),
      _sendResetPasswordEmailActionFactory.Create());
    await runner.Run(new(initiatePasswordResetData.Email), ct);
  }

  public async Task ResetPassword(ResetPasswordData resetPasswordData, CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriterRunner(
      _resetPasswordActionFactory.Create());
    await runner.Run(new(
      resetPasswordData.Email, resetPasswordData.Token, resetPasswordData.Password
      ), ct);
  }
}
