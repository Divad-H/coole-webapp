using CooleWebapp.Database.Configuration;
using CooleWebapp.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CooleWebapp.Database
{
  public static class DbSetup
  {
    public static IServiceCollection AddCooleWebappDatabase(
      this IServiceCollection serviceDescriptors,
      IConfigurationRoot configurationBuilder)
    {
      serviceDescriptors
        .AddDbContextFactory<MyContext>((sp, options) =>
        {
          var config = sp.GetRequiredService<IOptions<DatabaseConfig>>();
          var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
          string dbPath = Path.Join(path, $"{config.Value.DatabaseName}.sqlite");
          Directory.GetParent(dbPath)?.Create();
          options.UseSqlite($"Data Source={dbPath}");
        })
        .Configure<DatabaseConfig>(configurationBuilder.GetSection(nameof(DatabaseConfig)));
      return serviceDescriptors;
    }

    public static async Task InitializeCooleWebappDatabase(IServiceProvider serviceProvider, CancellationToken ct)
    {
      using var dbContext = await serviceProvider.GetRequiredService<IDbContextFactory<MyContext>>().CreateDbContextAsync(ct);
      await dbContext.Database.MigrateAsync(ct);
    }
  }
}