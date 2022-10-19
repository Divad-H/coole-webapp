using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Auth;

public static class Setup
{
  public static void AddWebappAuth(this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IUserRegistration, UserRegistration>();
    serviceDescriptors.AddScoped<IUserManager, UserManager>();
  }
}
