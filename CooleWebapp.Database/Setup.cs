using CooleWebapp.Database.Configuration;
using CooleWebapp.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using CooleWebapp.Auth.Model;
using CooleWebapp.Auth.Registration;
using CooleWebapp.Database.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Database.Runners;

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
          var config = sp.GetRequiredService<IOptions<DatabaseConfig>>();
          var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
          string dbPath = Path.Join(path, $"{config.Value.DatabaseName}.sqlite");
          Directory.GetParent(dbPath)?.Create();
          options.UseSqlite($"Data Source={dbPath}");
          options.UseOpenIddict();
        })
        .Configure<DatabaseConfig>(configurationBuilder.GetSection(nameof(DatabaseConfig)));
      serviceDescriptors.AddScoped<IUserDataAccess, UserDataAccess>();
      serviceDescriptors.AddScoped<IRunnerFactory, RunnerFactory>();
      return serviceDescriptors;
    }

    const string testUserEmail = "Karl@djimail.de";
    const string testUserName = "Karl";
    const string testUserPassword = "Ratte@1";

    public static async Task InitializeCooleWebappDatabase(IServiceProvider serviceProvider, CancellationToken ct)
    {
      await using var scope = serviceProvider.CreateAsyncScope();

      var dbContext = scope.ServiceProvider.GetRequiredService<WebappDbContext>();
      await dbContext.Database.MigrateAsync(ct);

      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WebappUser>>();
      var user = await userManager.FindByNameAsync(testUserName);
      if (user is null) 
      {
        user = new WebappUser { UserName = testUserName, Email = testUserEmail };
        await userManager.CreateAsync(user, testUserPassword);
      }
    }
  }
}
