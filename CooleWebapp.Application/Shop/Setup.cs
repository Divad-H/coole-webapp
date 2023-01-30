using CooleWebapp.Application.Shop.Actions;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Application.Shop
{
  public static class Setup
  {
    public static void AddShopServices(
      this IServiceCollection serviceDescriptors)
    {
      serviceDescriptors.AddScoped<IProducts, Services.Products>();
      serviceDescriptors.AddScopedFactory<BuyProductsAction>();
    }
  }
}
