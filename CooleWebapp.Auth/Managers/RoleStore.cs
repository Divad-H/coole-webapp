using CooleWebapp.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CooleWebapp.Auth.Managers
{
  public class RoleStore : RoleStore<IdentityRole>
  {
    public RoleStore(IdentityDbContext<WebappUser> context, IdentityErrorDescriber? describer = null)
      : base(context, describer)
    {
      AutoSaveChanges = false;
    }
  }
}
