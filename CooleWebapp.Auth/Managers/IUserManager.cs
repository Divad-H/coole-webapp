using CooleWebapp.Auth.Model;

namespace CooleWebapp.Auth.Managers;

public interface IUserManager
{
  public Task<WebappUser> FindByEmailAsync(string email);
  public Task CreateAsync(WebappUser user, string password);
  public Task DeleteAsync(WebappUser user);
}
