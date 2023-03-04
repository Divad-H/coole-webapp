using CooleWebapp.Auth.Model;

namespace CooleWebapp.Auth.Managers;

public interface IUserManager
{
  public Task<WebappUser?> FindByEmailAsync(string email);
  public Task CreateAsync(WebappUser user, string password);
  public Task DeleteAsync(WebappUser user);
  public Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser);
  public Task ConfirmEmailAsync(WebappUser webappUser, string token);
  public Task<string> GeneratePasswordResetTokenAsync(WebappUser user);
  public Task ResetPasswordAsync(WebappUser user, string token, string newPassword);
  public Task<IList<WebappUser>> GetUsersInRoleAsync(string roleName);
}
