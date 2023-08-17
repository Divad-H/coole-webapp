using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;

namespace CooleWebapp.Auth.Test.Mocks;

internal class UserManager : IUserManager
{
  public virtual Task ConfirmEmailAsync(WebappUser webappUser, string token)
  {
    Assert.Fail("Not expected to be called.");
    throw new InvalidOperationException();
  }

  public virtual Task CreateAsync(WebappUser user, string password)
  {
    Assert.Fail("Not expected to be called.");
    throw new InvalidOperationException();
  }

  public virtual Task DeleteAsync(WebappUser user)
  {
    Assert.Fail("Not expected to be called.");
    throw new InvalidOperationException();
  }

  public virtual Task<WebappUser?> FindByEmailAsync(string email)
  {
    Assert.Fail("Not expected to be called.");
    throw new InvalidOperationException();
  }

  public Task<WebappUser?> FindByIdAsync(string id)
  {
    throw new NotImplementedException();
  }

  public virtual Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser)
  {
    Assert.Fail("Not expected to be called.");
    throw new InvalidOperationException();
  }

  public virtual Task<string> GeneratePasswordResetTokenAsync(WebappUser user)
  {
    Assert.Fail("Not expected to be called.");
    throw new NotImplementedException();
  }

  public Task<IList<WebappUser>> GetUsersInRoleAsync(string roleName)
  {
    Assert.Fail("Not expected to be called.");
    throw new NotImplementedException();
  }

  public virtual Task ResetPasswordAsync(WebappUser user, string token, string newPassword)
  {
    Assert.Fail("Not expected to be called.");
    throw new NotImplementedException();
  }

  public Task SetUserRoles(string id, IReadOnlyCollection<string> roles, CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}
