using CooleWebapp.Statistics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Statistics;

public static class Setup
{
  public static void AddStatisticsServices(
    this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IStatisticsService, StatisticsService>();
  }
}
