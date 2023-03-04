using CooleWebapp.Auth.Registration;
using CooleWebapp.Backend.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RegistrationController : ControllerBase
{
  private readonly IUserRegistration _userRegistration;
  public RegistrationController(IUserRegistration userRegistration)
  {
    _userRegistration = userRegistration;
  }

  /// <summary>
  /// Start the registration of a new user
  /// </summary>
  /// <param name="registrationData">Information required for the registration</param>
  /// <param name="ct">Allows aborting the operation</param>
  /// <returns>The task of the operation</returns>
  [Route("register")]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status403Forbidden)]
  [HttpPost]
  public Task RegisterUser(RegistrationData registrationData, CancellationToken ct)
  {
    return _userRegistration.RegisterUser(
      registrationData,
      (data) =>
      {
        string url = $"{Request.Scheme}://{Request.Host}/auth/confirm-email";
        var param = new Dictionary<string, string?>() { 
          { "token", data.Token } ,
          { "email", data.Email } 
        };
        return QueryHelpers.AddQueryString(url, param);
      },
      ct);
  }

  [Route("confirm-email")]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [HttpGet]
  public Task ConfirmEmail([Required]string token, [Required] string email, CancellationToken ct)
  {
    return _userRegistration.ConfirmEmailAsync(email, token, ct);
  }

  [Route("initiate-password-reset")]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [HttpPost]
  public Task InitiatePasswordReset(InitiatePasswordResetData initiatePasswordResetData, CancellationToken ct)
  {
    return _userRegistration.InitiatePasswordReset(
      initiatePasswordResetData,
      data =>
      {
        string url = $"{Request.Scheme}://{Request.Host}/auth/set-new-password";
        var param = new Dictionary<string, string?>() {
          { "token", data.Token } ,
          { "email", data.Email }
        };
        return QueryHelpers.AddQueryString(url, param);
      },
      ct);
  }

  [Route("reset-password")]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [HttpPost]
  public Task ResetPassword(ResetPasswordData resetPasswordData, CancellationToken ct)
  {
    return _userRegistration.ResetPassword(resetPasswordData, ct);
  }
}
