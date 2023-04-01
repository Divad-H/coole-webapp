using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Repository;

public interface IUserDataAccess
{
  Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct);
  Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct);
  Task<CoolUser?> GetUser(UInt64 coolUserId, CancellationToken ct);
  IQueryable<CoolUser> GetAllUsers();
}
