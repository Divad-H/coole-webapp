using CooleWebapp.Application.Accounting.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Application.Accounting;
public static class Setup
{
  public static void AddAccountingServices(
    this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IUserAccount, UserAccount>();
  }
}
