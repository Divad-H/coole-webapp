using CooleWebapp.Application.Accounting.Services;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
    [ProducesDefaultResponseType(typeof(UserBalanceResponseModel))]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public Task<UserBalanceResponseModel> GetBalance(CancellationToken ct)
    {
      var userId = User.FindFirstValue(Claims.Subject) 
        ?? throw new ClientError(ErrorType.NotFound, "User not found.");
      return _userAccount.GetUserBalance(userId, ct);
    }

    [Route("AddBalance")]
    [ProducesDefaultResponseType(typeof(UserBalanceResponseModel))]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status403Forbidden)]
    [HttpPost]
    public Task<UserBalanceResponseModel> AddBalance(
      AddBalanceRequestModel addBalanceRequestModel,
      CancellationToken ct)
    {
      var userId = User.FindFirstValue(Claims.Subject) 
        ?? throw new ClientError(ErrorType.NotFound, "User not found.");
      return _userAccount.AddBalance(userId, addBalanceRequestModel.Amount, ct);
    }
  }
}
