using CooleWebapp.Application.ImageService;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Images;

public static class Setup
{
  public static void AddImageServices(this IServiceCollection serviceDescriptors)
  {
    serviceDescriptors.AddScoped<IImageValidator, ImageValidator>();
  }
}
