using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Registration;
using CooleWebapp.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Auth;

public static class Setup
{
  public static void AddWebappAuth(this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IUserRegistration, UserRegistration>();
    serviceDescriptors.AddScoped<IUserManager, UserManager>();
    serviceDescriptors.AddScopedFactory<RegisterUserAction>();
    serviceDescriptors.AddScopedFactory<SendConfirmationRequestEmailAction>();
    serviceDescriptors.AddScopedFactory<ConfirmEmailAction>();
    serviceDescriptors.AddScopedFactory<SendResetPasswordEmailAction>();
    serviceDescriptors.AddScopedFactory<StartInitiatePasswordResetAction>();
    serviceDescriptors.AddScopedFactory<ResetPasswordAction>();
  }
}
