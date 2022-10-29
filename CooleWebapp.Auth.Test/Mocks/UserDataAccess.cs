using CooleWebapp.Auth.Registration;
using CooleWebapp.Core.Entities;

namespace CooleWebapp.Auth.Test.Mocks
{
  internal class UserDataAccess : IUserDataAccess
  {
    private readonly List<CoolUser> _createdUsers = new();
    public IReadOnlyCollection<CoolUser> CreatedUsers => _createdUsers;
    public Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct)
    {
      var newUser = user with { Id = 1 };
      _createdUsers.Add(user);
      return Task.FromResult(user);
    }
  }
}
