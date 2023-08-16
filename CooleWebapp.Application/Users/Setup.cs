using CooleWebapp.Application.Users.Actions;
using CooleWebapp.Application.Users.Services;
using CooleWebapp.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Application.Users;

public static class Setup
{
  public static void AddUsersServices(
  this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IUserSettingsService, UserSettingsService>();
    serviceDescriptors.AddScoped<IAdminUsersService, AdminUsersService>();
    serviceDescriptors.AddScopedFactory<UpdateUserSettingsAction>();
  }
}
