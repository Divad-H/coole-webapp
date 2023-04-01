using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Entities;

namespace CooleWebapp.Auth.Test.Mocks
{
  internal class UserDataAccess : IUserDataAccess
  {
    private readonly List<CoolUser> _createdUsers = new();
    public IReadOnlyCollection<CoolUser> CreatedUsers => _createdUsers;
    public Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct)
    {
      _createdUsers.Add(user);
      return Task.FromResult(user);
    }

    public Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct)
    {
      return Task.FromResult(CreatedUsers.FirstOrDefault(u => u.WebappUserId == webappUserId));
    }

    public IQueryable<CoolUser> GetAllUsers()
    {
      throw new NotImplementedException();
    }

    public Task<CoolUser?> GetUser(ulong coolUserId, CancellationToken ct)
    {
      throw new NotImplementedException();
    }
  }
}
