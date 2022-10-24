using CooleWebapp.Auth.Model;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace CooleWebapp.Auth.Managers;

internal sealed class UserManager : IUserManager
{
  private readonly UserManager<WebappUser> _userManager;
  public UserManager(UserManager<WebappUser> userManager)
    => _userManager = userManager;

  private static void ThrowOnError(IdentityResult identityResult)
  {
    if (!identityResult.Succeeded)
      throw new ClientError(
        ErrorType.InvalidOperation,
        string.Join('\n', identityResult.Errors.Select(e => e.Description)));
  }

  public async Task CreateAsync(WebappUser user, string password)
    => ThrowOnError(await _userManager.CreateAsync(user, password));

  public async Task DeleteAsync(WebappUser user)
    => ThrowOnError(await _userManager.DeleteAsync(user));

  public Task<WebappUser> FindByEmailAsync(string email)
    => _userManager.FindByEmailAsync(email);

  public Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser)
    => _userManager.GenerateEmailConfirmationTokenAsync(webappUser);

  public async Task ConfirmEmailAsync(WebappUser webappUser, string token)
    => ThrowOnError(await _userManager.ConfirmEmailAsync(webappUser, token));
}

