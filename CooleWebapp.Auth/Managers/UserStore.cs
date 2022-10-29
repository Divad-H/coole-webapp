using CooleWebapp.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Auth.Managers
{
  public class UserStore : Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore<WebappUser>
  {
    public UserStore(
      IdentityDbContext<WebappUser> context, 
      IdentityErrorDescriber? describer = null) : base(context, describer)
    {
      AutoSaveChanges = false;
    }
  }
}
