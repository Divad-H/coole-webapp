using CooleWebapp.Database.Model;
using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CooleWebapp.Application.Users.Repository;

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

  public Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct)
  {
    return _dbContext.CoolUsers.FirstOrDefaultAsync(u => u.WebappUserId == webappUserId, ct);
  }
}
