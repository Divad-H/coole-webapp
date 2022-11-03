using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;

namespace CooleWebapp.Auth.Test.Mocks
{
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

    public virtual Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser)
    {
      Assert.Fail("Not expected to be called.");
      throw new InvalidOperationException();
    }

    public Task<string> GeneratePasswordResetTokenAsync(WebappUser user)
    {
      Assert.Fail("Not expected to be called.");
      throw new NotImplementedException();
    }
  }
}
