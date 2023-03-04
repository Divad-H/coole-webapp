using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Application.Users.Services;
using CooleWebapp.Auth.DefaultUsers;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.PinCode;
using CooleWebapp.Auth.Registration;
using CooleWebapp.Core.Utilities;
using Microsoft.AspNetCore.Identity;
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
    serviceDescriptors.AddScoped<IUserFilters, UserFilters>();
    serviceDescriptors.AddScopedFactory<RegisterUserAction>();
    serviceDescriptors.AddScopedFactory<SendConfirmationRequestEmailAction>();
    serviceDescriptors.AddScopedFactory<ConfirmEmailAction>();
    serviceDescriptors.AddScopedFactory<SendResetPasswordEmailAction>();
    serviceDescriptors.AddScopedFactory<StartInitiatePasswordResetAction>();
    serviceDescriptors.AddScopedFactory<ResetPasswordAction>();

    serviceDescriptors.Configure<AdministratorsConfiguration>(
      configurationBuilder.GetSection(nameof(AdministratorsConfiguration)));
    serviceDescriptors.AddScoped<IPinCodeHashing, PinCodeHashing>();
    serviceDescriptors.AddScoped<IPasswordHasher<DummyUser>, PasswordHasher<DummyUser>>();
  }
}
