using CooleWebapp.Application.Accounting.Services;
using CooleWebapp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;
using static AspNet.Security.OpenIdConnect.Primitives.OpenIdConnectConstants;

namespace CooleWebapp.Backend.Controllers
{
  [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.User)]
  [ApiController]
  [Route("[controller]")]
  public class UserAccountController : ControllerBase
  {
    private IUserAccount _userAccount;

    public UserAccountController(IUserAccount userAccount)
    {
      _userAccount = userAccount;
    }

    [Route("GetBalance")]
    [HttpGet]
    public Task<UserBalanceResponseModel> GetBalance(CancellationToken ct)
    {
      var userId = User.FindFirstValue(Claims.Subject);
      return _userAccount.GetUserBalance(userId, ct);
    }
  }
}
