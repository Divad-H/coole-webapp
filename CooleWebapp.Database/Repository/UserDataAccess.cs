using CooleWebapp.Auth.Registration;
using CooleWebapp.Database.Model;
using Microsoft.EntityFrameworkCore;
using CooleWebapp.Core.Entities;

namespace CooleWebapp.Database.Repository;

public sealed class UserDataAccess : IUserDataAccess
{
  private readonly IDbContextFactory<WebappDbContext> _dbContextFactory;
  public UserDataAccess(IDbContextFactory<WebappDbContext> dbContextFactory)
  {
    _dbContextFactory = dbContextFactory;
  }

  public async Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct)
  {
    using var context = await _dbContextFactory.CreateDbContextAsync(ct);
    var res = await context.CoolUsers.AddAsync(user, ct);
    await context.SaveChangesAsync(ct);
    return res.Entity;
  }
}
