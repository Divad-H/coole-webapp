using CooleWebapp.Auth.Model;
using CooleWebapp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

  public DbSet<CoolUser> CoolUsers { get; set; }
}
