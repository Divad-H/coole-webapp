using Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Model
{
  public sealed class MyContext : DbContext
  {
    public MyContext(DbContextOptions<MyContext> dbContextOptions)
      : base(dbContextOptions)
    { }

    public DbSet<User>? Users { get; set; }
  }
}
