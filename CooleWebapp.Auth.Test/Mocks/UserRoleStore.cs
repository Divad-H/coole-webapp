using CooleWebapp.Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace CooleWebapp.Auth.Test.Mocks
{
  internal class UserRoleStore : IUserRoleStore<WebappUser>
  {
    public Task AddToRoleAsync(WebappUser user, string roleName, CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    public Task<IdentityResult> CreateAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public Task<WebappUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<WebappUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IList<WebappUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(WebappUser user, string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task RemoveFromRoleAsync(WebappUser user, string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(WebappUser user, string? normalizedName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task SetUserNameAsync(WebappUser user, string? userName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(WebappUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
