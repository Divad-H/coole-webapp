using CooleWebapp.Core.BusinessActionRunners;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly IRunnerFactory _runnerFactory;
  private readonly IServiceProvider _serviceProvider;
  public UserRegistration(
    IRunnerFactory runnerFactory,
    IServiceProvider serviceProvider)
  {
    _runnerFactory = runnerFactory;
    _serviceProvider = serviceProvider;
  }

  public async Task RegisterUser(
    RegistrationData registrationData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct)
  {
    var runner = _runnerFactory.CreateTransaction2Runner(
      _serviceProvider.GetRequiredService<RegisterUserAction>(),
      registrationRes => new SendConfirmationRequestDto(
        createEmailLink((registrationRes.Token, registrationData.Email)), 
        registrationData.Name, 
        registrationData.Email),
      _serviceProvider.GetRequiredService<SendConfirmationRequestEmailAction>());
    await runner.Run(registrationData, ct);
  }

  public async Task ConfirmEmailAsync(string email, string token, CancellationToken ct)
  {
    var runner = _runnerFactory.CreateWriterRunner(
      _serviceProvider.GetRequiredService<ConfirmEmailAction>());
    await runner.Run(new(email, token), ct);
  }
}
