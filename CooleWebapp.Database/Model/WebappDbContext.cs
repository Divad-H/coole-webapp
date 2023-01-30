using CooleWebapp.Auth.Model;
using CooleWebapp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

#nullable disable

namespace CooleWebapp.Database.Model;

public sealed class WebappDbContext : IdentityDbContext<WebappUser>
{
  public WebappDbContext(DbContextOptions<WebappDbContext> dbContextOptions)
    : base(dbContextOptions)
  { }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    base.ConfigureConventions(configurationBuilder);
    configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeUtcConverter>();
  }
  public DbSet<CoolUser> CoolUsers { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<ProductImage> ProductImages { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<MonthlyClosing> MonthlyClosings { get; set; }
  public DbSet<Balance> Balance { get; set; }
}

public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
{
  public DateTimeUtcConverter() : base(
    x => x,
    x => DateTime.SpecifyKind(x, DateTimeKind.Utc), 
    null)
  { }
}
