using CooleWebapp.Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace CooleWebapp.Auth.Managers;

public interface IUserManager
{
  public Task<WebappUser?> FindByEmailAsync(string email);
  public Task CreateAsync(WebappUser user, string password);
  public Task DeleteAsync(WebappUser user);
  public Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser);
  public Task ConfirmEmailAsync(WebappUser webappUser, string token);
  public Task<string> GeneratePasswordResetTokenAsync(WebappUser user);
  public Task<IdentityResult> ResetPasswordAsync(WebappUser user, string token, string newPassword);
}
