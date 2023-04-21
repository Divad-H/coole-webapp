using CooleWebapp.Database.Configuration;
using CooleWebapp.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using CooleWebapp.Auth.Model;
using CooleWebapp.Database.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Database.Runners;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CooleWebapp.Core.Entities;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Utilities;
using CooleWebapp.Database.Pagination;

namespace CooleWebapp.Database
{
  public static class DbSetup
  {
    public static IServiceCollection AddCooleWebappDatabase(
      this IServiceCollection serviceDescriptors,
      IConfigurationRoot configurationBuilder)
    {
      serviceDescriptors
        .AddDbContext<WebappDbContext>((sp, options) =>
        {
          var config = sp.GetRequiredService<IOptions<DatabaseConfig>>().Value;
          options.UseSqlServer(config.DatabaseConnectionString);
          options.UseOpenIddict();
        })
        .Configure<DatabaseConfig>(configurationBuilder.GetSection(nameof(DatabaseConfig)));
      serviceDescriptors.AddScoped<IdentityDbContext<WebappUser>>(sp => sp.GetRequiredService<WebappDbContext>());
      serviceDescriptors.AddScoped<IUserDataAccess, UserDataAccess>();
      serviceDescriptors.AddScoped<IUserSettingsDataAccess, UserSettingsDataAccess>();
      serviceDescriptors.AddScoped<IProductDataAccess, ProductsDataAccess>();
      serviceDescriptors.AddScoped<IAccountingDataAccess, AccountingDataAccess>();
      serviceDescriptors.AddScoped<IRunnerFactory, RunnerFactory>();
      serviceDescriptors.AddScoped<IQueryPaginated, QueryPaginated>();
      return serviceDescriptors;
    }

    private static readonly IReadOnlyCollection<string> _roles 
      = new[] { Roles.Registered, Roles.User, Roles.Fridge, Roles.Administrator };

    public static async Task InitializeCooleWebappDatabase(IServiceProvider serviceProvider, CancellationToken ct)
    {
      await using var scope = serviceProvider.CreateAsyncScope();

      var config = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
      if (!config.InitializeDatabase)
        return;

      var dbContext = scope.ServiceProvider.GetRequiredService<WebappDbContext>();
      await dbContext.Database.MigrateAsync(ct);

      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      foreach(var role in _roles)
        if (await roleManager.FindByNameAsync(role) is null)
          await roleManager.CreateAsync(new(role));

      await dbContext.SaveChangesAsync(ct);
    }
  }
}
