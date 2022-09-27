using CooleWebapp.Auth.Model;
using CooleWebapp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Model
{
  public sealed class WebappDbContext : IdentityDbContext<WebappUser>
  {
    public WebappDbContext(DbContextOptions<WebappDbContext> dbContextOptions)
      : base(dbContextOptions)
    { }
  }
}
