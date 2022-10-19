using CooleWebapp.Core.Entities;

namespace CooleWebapp.Auth.Registration;

public interface IUserDataAccess
{
  Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct);
}
