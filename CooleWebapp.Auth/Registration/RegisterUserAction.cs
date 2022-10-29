using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Registration;

public class RegisterUserAction : IBusinessAction<RegistrationData, UserRegistrationOutDto>
{
  private readonly IUserManager _userManager;
  private readonly IUserDataAccess _userDataAccess;
  public RegisterUserAction(
    IUserManager userManager,
    IUserDataAccess userDataAccess)
  {
    _userManager = userManager;
    _userDataAccess = userDataAccess;
  }

  public async Task<UserRegistrationOutDto> Run(RegistrationData registrationData, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(registrationData.Email);
    if (user is not null)
      throw new ClientError(ErrorType.InvalidOperation, $"A user with the email {registrationData.Email} already exists.");
    user = new WebappUser() { Email = registrationData.Email, UserName = registrationData.Email };
    await _userManager.CreateAsync(user, registrationData.Password);
    user = await _userManager.FindByEmailAsync(registrationData.Email);
    await _userDataAccess.CreateUser(new()
    {
      Name = registrationData.Name,
      Initials = registrationData.Initials,
      Title = registrationData.Title,
      WebappUserId = user.Id
    }, ct);
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    return new(token);
  }
}
