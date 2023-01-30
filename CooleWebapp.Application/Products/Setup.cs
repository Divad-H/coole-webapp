using CooleWebapp.Application.Products.Actions;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Application.Products
{
  public static class Setup
  {
    public static void AddProductsServices(
      this IServiceCollection serviceDescriptors)
    {
      serviceDescriptors.AddScoped<IAdminProducts, AdminProducts>();
      serviceDescriptors.AddScopedFactory<CreateProductAction>();
      serviceDescriptors.AddScopedFactory<UpdateProductAction>();
      serviceDescriptors.AddScopedFactory<DeleteProductAction>();
    }
  }
}
