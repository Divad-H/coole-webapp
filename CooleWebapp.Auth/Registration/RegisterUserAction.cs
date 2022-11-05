using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace CooleWebapp.Auth.Registration;

public class RegisterUserAction : IBusinessAction<RegistrationData, UserRegistrationOutDto>
{
  private readonly IUserManager _userManager;
  private readonly IUserDataAccess _userDataAccess;
  IUserRoleStore<WebappUser> _userRoleStore;
  public RegisterUserAction(
    IUserRoleStore<WebappUser> userRoleStore,
    IUserManager userManager,
    IUserDataAccess userDataAccess)
  {
    _userRoleStore = userRoleStore;
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
    await _userRoleStore.AddToRoleAsync(user, Roles.User.ToUpper(), ct);
    await _userRoleStore.AddToRoleAsync(user, Roles.Registered.ToUpper(), ct);
    await _userDataAccess.CreateUser(new()
    {
      Name = registrationData.Name,
      Initials = registrationData.Initials,
      Title = registrationData.Title,
      WebappUserId = user!.Id
    }, ct);
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    return new(token);
  }
}
