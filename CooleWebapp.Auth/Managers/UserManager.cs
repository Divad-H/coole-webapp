using CooleWebapp.Auth.Model;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace CooleWebapp.Auth.Managers;

internal sealed class UserManager : IUserManager
{
  private readonly UserManager<WebappUser> _userManager;
  private readonly IUserRoleStore<WebappUser> _userRoleStore;
  public UserManager(
    UserManager<WebappUser> userManager,
    IUserRoleStore<WebappUser> userRoleStore)
  {
    _userManager = userManager;
    _userRoleStore = userRoleStore;
  }

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

  public Task<WebappUser?> FindByEmailAsync(string email)
    => _userManager.FindByEmailAsync(email)!;

  public Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser)
    => _userManager.GenerateEmailConfirmationTokenAsync(webappUser);

  public async Task ConfirmEmailAsync(WebappUser webappUser, string token)
    => ThrowOnError(await _userManager.ConfirmEmailAsync(webappUser, token));

  public Task<string> GeneratePasswordResetTokenAsync(WebappUser user)
    => _userManager.GeneratePasswordResetTokenAsync(user);

  public async Task ResetPasswordAsync(WebappUser user, string token, string newPassword)
    => ThrowOnError(await _userManager.ResetPasswordAsync(user, token, newPassword));

  public Task<IList<WebappUser>> GetUsersInRoleAsync(string roleName)
    => _userManager.GetUsersInRoleAsync(roleName);

  public async Task SetUserRoles(string id, IReadOnlyCollection<string> roles, CancellationToken ct)
  {
    var user = await _userManager.FindByIdAsync(id);
    if (user is null)
    {
      throw new ClientError(ErrorType.NotFound, "The user did not exist.");
    }

    async Task updateRole(string roleName)
    {
      var isSelected = roles.Contains(roleName);
      var normalizedRole = _userManager.NormalizeName(roleName);
      if (await _userRoleStore.IsInRoleAsync(user!, normalizedRole, ct).ConfigureAwait(false))
      {
        if (!isSelected)
        {
          await _userManager.RemoveFromRoleAsync(user!, normalizedRole);
        }
      }
      else
      {
        if (isSelected)
        {
          await _userManager.AddToRoleAsync(user!, normalizedRole);
        }
      }
    }

    await updateRole(Roles.Fridge);
    await updateRole(Roles.User);
    await updateRole(Roles.Administrator);

    await _userManager.UpdateAsync(user);
  }
}

