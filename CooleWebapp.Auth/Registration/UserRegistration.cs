using CooleWebapp.Auth.Model;
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
      throw new InvalidOperationException("A user with this email already exists.");
    user = new WebappUser() { Email = registrationData.Email, UserName = registrationData.Email };
    var identityResult = await _userManager.CreateAsync(user, registrationData.Password);
    if (!identityResult.Succeeded)
      throw new InvalidOperationException(string.Join('\n', identityResult.Errors.Select(e => e.Description)));
    try
    {
      user = await _userManager.FindByEmailAsync(registrationData.Email);
      var initials = $"{registrationData.FirstName.First()}{registrationData.LastName.First()}{registrationData.LastName.Last()}";
      await _userDataAccess.CreateUser(new()
      {
        FirstName = registrationData.FirstName,
        LastName = registrationData.LastName,
        Title = registrationData.Title,
        WebappUserId = user.Id,
        Initials = initials
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
