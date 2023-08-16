using CooleWebapp.Application.Users.Services;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CooleWebapp.Backend.Controllers;


[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.Administrator)]
[ApiController]
[Route("[controller]")]
public class AdminUsersController : ControllerBase
{
  private readonly IAdminUsersService _adminUsers;
  public AdminUsersController(
    IAdminUsersService adminUsers)
  {
    _adminUsers = adminUsers;
  }


  [Route("GetUsers")]
  [ProducesDefaultResponseType(typeof(GetUsersResponseModel))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpGet]
  public Task<GetUsersResponseModel> GetUsers(
    [FromQuery] GetUsersRequestModel data,
    CancellationToken ct)
  {
    return _adminUsers.ReadUsers(data, ct);
  }

  [Route("DeleteUser/{userId}")]
  [ProducesDefaultResponseType(typeof(UInt64))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpDelete]
  public Task DeleteUser([FromRoute] UInt64 userId, CancellationToken ct)
  {
    throw new ClientError(ErrorType.InvalidOperation, "Not implemented");
    //return _adminUsers.DeleteUser(userId, ct);
  }
}
