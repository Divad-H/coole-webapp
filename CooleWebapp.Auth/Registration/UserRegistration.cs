using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly IUserManager _userManager;
  private readonly IUserDataAccess _userDataAccess;
  public UserRegistration(
    IUserManager userManager,
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
    await _userManager.CreateAsync(user, registrationData.Password);
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
