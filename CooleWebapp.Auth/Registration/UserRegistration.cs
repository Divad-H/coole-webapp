using CooleWebapp.Auth.Model;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly UserManager<WebappUser> _userManager;
  private readonly IUserDataAccess _userDataAccess;
  public UserRegistration(
    UserManager<WebappUser> userManager,
    IUserDataAccess userDataAccess)
  {
    _userManager = userManager;
    _userDataAccess = userDataAccess;
  }

  public async Task RegisterUser(RegistrationData registrationData, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(registrationData.Email);
    if (user is not null)
      throw new ClientError(ErrorType.InvalidOperation, $"A user with the email {registrationData.Email} already exists.");
    user = new WebappUser() { Email = registrationData.Email, UserName = registrationData.Email };
    var identityResult = await _userManager.CreateAsync(user, registrationData.Password);
    if (!identityResult.Succeeded)
      throw new ClientError(ErrorType.InvalidOperation, string.Join('\n', identityResult.Errors.Select(e => e.Description)));
    try
    {
      user = await _userManager.FindByEmailAsync(registrationData.Email);
      await _userDataAccess.CreateUser(new()
      {
        Name = registrationData.Name,
        Initials = registrationData.Initials,
        Title = registrationData.Title,
        WebappUserId = user.Id
      }, ct);
    }
    catch (Exception)
    {
      await _userManager.DeleteAsync(user);
      throw;
    }
    // TODO: generate email confirmation token and send...
  }
}
