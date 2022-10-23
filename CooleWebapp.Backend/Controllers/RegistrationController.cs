using CooleWebapp.Auth.Registration;
using CooleWebapp.Backend.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

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
    return _userRegistration.RegisterUser(registrationData, ct);
  }
}
