using CooleWebapp.Application.Dashboard.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Application.Dashboard
{
  public static class Setup
  {
    public static void AddDashboardServices(
      this IServiceCollection serviceDescriptors)
    {
      serviceDescriptors.AddScoped<IDashboardService, Services.DashboardService>();
    }
  }
}
