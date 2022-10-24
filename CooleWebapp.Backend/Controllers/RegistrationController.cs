using CooleWebapp.Auth.Registration;
using CooleWebapp.Backend.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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
  [HttpPost]
  public Task RegisterUser(RegistrationData registrationData, CancellationToken ct)
  {
    return _userRegistration.RegisterUser(
      registrationData,
      (data) =>
      {
        string url = $"{Request.Scheme}://{Request.Host}/Registration/confirm-email";
        var param = new Dictionary<string, string?>() { 
          { "token", data.Token } ,
          { "email", data.Email } 
        };
        return QueryHelpers.AddQueryString(url, param);
      },
      ct);
  }

  [Route("confirm-email")]
  [HttpGet]
  public Task ConfirmEmail(string token, string email, CancellationToken ct)
  {
    return _userRegistration.ConfirmEmailAsync(email, token, ct);
  }
}
