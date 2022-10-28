using CooleWebapp.Auth.Registration;
using CooleWebapp.Database.Model;
using CooleWebapp.Core.Entities;

namespace CooleWebapp.Database.Repository;

public sealed class UserDataAccess : IUserDataAccess
{
  private readonly WebappDbContext _dbContext;
  public UserDataAccess(WebappDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct)
  {
    var res = await _dbContext.CoolUsers.AddAsync(user, ct);
    return res.Entity;
  }
}
