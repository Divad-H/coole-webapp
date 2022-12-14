using CooleWebapp.Auth.DefaultUsers;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Registration;
using CooleWebapp.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Auth;

public static class Setup
{
  public static void AddWebappAuth(
    this IServiceCollection serviceDescriptors,
    IConfigurationRoot configurationBuilder)
  {
    serviceDescriptors.AddScoped<IUserRegistration, UserRegistration>();
    serviceDescriptors.AddScoped<IUserManager, UserManager>();
    serviceDescriptors.AddScopedFactory<RegisterUserAction>();
    serviceDescriptors.AddScopedFactory<SendConfirmationRequestEmailAction>();
    serviceDescriptors.AddScopedFactory<ConfirmEmailAction>();
    serviceDescriptors.AddScopedFactory<SendResetPasswordEmailAction>();
    serviceDescriptors.AddScopedFactory<StartInitiatePasswordResetAction>();
    serviceDescriptors.AddScopedFactory<ResetPasswordAction>();

    serviceDescriptors.Configure<AdministratorsConfiguration>(
      configurationBuilder.GetSection(nameof(AdministratorsConfiguration)));
  }
}
