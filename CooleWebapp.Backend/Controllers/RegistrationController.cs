using CooleWebapp.Auth.Registration;
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

  [HttpPost]
  public async Task RegisterUser(RegistrationData registrationData, CancellationToken ct)
  {
    await _userRegistration.RegisterUser(registrationData, ct);
  }
}
