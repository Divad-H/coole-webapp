using CooleWebapp.Application.Accounting.Actions;
using CooleWebapp.Application.Accounting.Services;
using CooleWebapp.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Application.Accounting;
public static class Setup
{
  public static void AddAccountingServices(
    this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IUserAccount, UserAccount>();
    serviceDescriptors.AddScopedFactory<AddBalanceAction>();
  }
}
